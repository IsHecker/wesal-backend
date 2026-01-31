using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Visitations;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Visitations;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.ObligationAlerts.MissedVisitationsDetection;

[DisallowConcurrentExecution]
internal sealed class DetectMissedVisitationsJob(
    IOptions<DetectMissedVisitationsOptions> options,
    IOptions<VisitationOptions> visitationOptions,
    IComplianceMetricsRepository metricsRepository,
    ObligationAlertService alertService,
    WesalDbContext dbContext) : IJob
{
    private readonly DetectMissedVisitationsOptions options = options.Value;
    private readonly VisitationOptions visitationOptions = visitationOptions.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var missedVisitations = await GetMissedVisitationsAsync(options.BatchSize, context.CancellationToken);

        foreach (var visitation in missedVisitations)
        {
            await ProcessVisitationAsync(visitation, context.CancellationToken);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }

    private Task<List<Visitation>> GetMissedVisitationsAsync(int batchSize, CancellationToken cancellationToken)
    {
        return dbContext.Visitations
            .Where(IsMissed)
            .Where(visit => visit.Status != VisitationStatus.Completed
                && visit.Status != VisitationStatus.Missed)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    private async Task ProcessVisitationAsync(Visitation visitation, CancellationToken cancellationToken)
    {
        visitation.MarkAsMissed();
        dbContext.Visitations.Update(visitation);

        await alertService.RecordViolationAsync(
            visitation.ParentId,
            ViolationType.MissedVisit,
            visitation.Id,
            $@"Missed the scheduled visitation 
            on {visitation.StartAt} at {visitation.StartAt.TimeOfDay}.",
            cancellationToken);

        await RecordVisitationMissedAsync(visitation, cancellationToken);
    }

    private async Task RecordVisitationMissedAsync(Visitation visitation, CancellationToken cancellationToken)
    {
        var metrics = await metricsRepository.GetAsync(
            visitation.FamilyId,
            visitation.ParentId,
            DateTime.UtcNow.ToDateOnly(),
            cancellationToken) ?? throw new InvalidOperationException();

        metrics.RecordVisitationMissed();
    }

    private Expression<Func<Visitation, bool>> IsMissed =>
        visit => DateTime.UtcNow >= visit.EndAt.AddMinutes(visitationOptions.CheckOutGracePeriodMinutes);
}