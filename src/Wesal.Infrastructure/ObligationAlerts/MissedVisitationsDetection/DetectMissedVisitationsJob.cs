using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Services;
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
    IObligationAlertService alertService,
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
            .AsTracking()
            .Where(visit => visit.Status != VisitationStatus.Completed
                && visit.Status != VisitationStatus.Missed)
            .Where(IsViolatingSchedule)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    private async Task ProcessVisitationAsync(Visitation visitation, CancellationToken cancellationToken)
    {
        if (visitation.Status == VisitationStatus.InProgress)
        {
            visitation.MarkAsOverstayedVisit();
        }
        else if (visitation.Status == VisitationStatus.PartiallyCheckedIn)
        {
            visitation.MarkAsPartiallyCompleted();
        }
        else
        {
            visitation.MarkAsMissed();
        }

        var violations = ResolveViolations(visitation);

        foreach (var (parentId, type, message) in violations)
        {
            await alertService.RecordViolationAsync(parentId, type, visitation.Id, message, cancellationToken);
        }
    }

    private static List<(Guid ParentId, ViolationType Type, string Message)> ResolveViolations(Visitation visitation)
    {
        var custodialId = visitation.VisitationSchedule.CustodialParentId;
        var nonCustodialId = visitation.NonCustodialParentId;
        var violations = new List<(Guid ParentId, ViolationType Type, string Message)>();

        if (visitation.Status == VisitationStatus.OverstayedVisit)
        {
            // Check who actually overstayed or failed to check out
            if (!visitation.Attendance.IsNonCustodialCheckedOut)
            {
                violations.Add((nonCustodialId, ViolationType.OverstayedVisit,
                    "Failed to check-out and conclude the visitation session on time."));
            }

            if (!visitation.Attendance.IsCompanionCheckedOut)
            {
                violations.Add((custodialId, ViolationType.OverstayedVisit,
                    "Failed to check-out and conclude the visitation session on time."));
            }

            return violations;
        }

        if (!visitation.Attendance.IsCompanionCheckedIn)
        {
            violations.Add((custodialId, ViolationType.MissedVisit, "The children were not delivered for the scheduled visitation session."));
        }

        if (!visitation.Attendance.IsNonCustodialCheckedIn)
        {
            violations.Add((nonCustodialId, ViolationType.MissedVisit, "The non-custodial parent failed to attend the scheduled visitation session."));
        }

        return violations;
    }

    private Expression<Func<Visitation, bool>> IsViolatingSchedule =>
        visit => ((visit.Status == VisitationStatus.Scheduled || visit.Status == VisitationStatus.PartiallyCheckedIn)
            && EgyptTime.Now > visit.StartAt.AddMinutes(visitationOptions.CheckInGracePeriodMinutes))
            || ((visit.Status == VisitationStatus.InProgress)
                && EgyptTime.Now > visit.EndAt.AddMinutes(visitationOptions.CheckOutGracePeriodMinutes));
}