using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Visitations;
using Wesal.Domain.Common;
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
            .Include(visit => visit.VisitationSchedule)
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

        var (parentIds, type, message) = ResolveViolation(visitation);

        foreach (var parentId in parentIds)
        {
            await alertService.RecordViolationAsync(parentId, type, visitation.Id, message, cancellationToken);
        }

        await RecordVisitationMissedAsync(visitation, cancellationToken);
    }

    private static (Guid[] parentIds, ViolationType type, string message) ResolveViolation(Visitation visitation)
    {
        //$@"Missed the scheduled visitation on {visitation.StartAt} at {visitation.StartAt.TimeOfDay}."
        //$@"Failed to end the visitation on time. 
        // The visit scheduled to end at {visitation.EndAt} exceeded the allowed grace period."

        var custodialId = visitation.VisitationSchedule.CustodialParentId;
        var nonCustodialId = visitation.NonCustodialParentId;

        // No one showed up
        if (!visitation.IsNonCustodialCheckedIn && !visitation.IsCompanionCheckedIn)
            return (
                [custodialId],
                ViolationType.MissedVisit,
                $@"Neither party attended the scheduled visitation on {visitation.StartAt}.");

        // Only companion missed the visit
        if (!visitation.IsCompanionCheckedIn)
            return (
                [custodialId],
                ViolationType.MissedVisit,
                $@"Children were not delivered for the visitation scheduled on {visitation.StartAt}.");

        // Only custodial parent missed the visit
        if (!visitation.IsNonCustodialCheckedIn)
            return (
                [nonCustodialId],
                ViolationType.MissedVisit,
                $@"Did not attend the visitation scheduled on {visitation.StartAt}.");

        // Both checked in but overstayed
        return (
            [custodialId, nonCustodialId],
            ViolationType.OverstayedVisit,
            $@"Failed to end the visitation on time. 
            The visit scheduled to end at {visitation.EndAt} exceeded the allowed grace period.");
    }

    private async Task RecordVisitationMissedAsync(Visitation visitation, CancellationToken cancellationToken)
    {
        var metrics = await metricsRepository.GetAsync(
            visitation.FamilyId,
            visitation.NonCustodialParentId,
            EgyptTime.Today,
            cancellationToken) ?? throw new InvalidOperationException();

        metrics.RecordVisitationMissed();
    }

    private Expression<Func<Visitation, bool>> IsMissed =>
        visit => EgyptTime.Now >= visit.EndAt.AddMinutes(visitationOptions.CheckOutGracePeriodMinutes);
}