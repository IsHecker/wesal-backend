using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Visitations;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.ObligationAlerts.MissedVisitationsDetection;

[DisallowConcurrentExecution]
internal sealed class DetectMissedVisitationsJob(
    IOptions<DetectMissedVisitationsOptions> options,
    ObligationAlertService alertService,
    WesalDbContext dbContext) : IJob
{
    private readonly DetectMissedVisitationsOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var missedVisitations = await GetMissedVisitationsAsync(options.BatchSize);

        foreach (var visitation in missedVisitations)
        {
            await ProcessVisitationAsync(visitation);
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private Task<List<Visitation>> GetMissedVisitationsAsync(int batchSize)
    {
        return dbContext.Visitations
            .Where(visit => !visit.IsCompleted())
            .Take(batchSize)
            .ToListAsync();
    }

    private async Task ProcessVisitationAsync(Visitation visitation)
    {
        visitation.MarkAsMissed();
        dbContext.Visitations.Update(visitation);

        await alertService.CreateAlertAsync(
            visitation.ParentId,
            AlertType.MissedVisit,
            visitation.Id,
            $@"missed the scheduled visitation 
            on {visitation.Date} at {visitation.StartTime}.");
    }
}