using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.ObligationAlerts;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Results;

namespace Wesal.Application.ObligationAlerts.ListObligationAlerts;

internal sealed class ListObligationAlertsQueryHandler(
    ICourtStaffRepository courtStaffRepository,
    IWesalDbContext context)
    : IQueryHandler<ListObligationAlertsQuery, ObligationAlertsResponse>
{
    public async Task<Result<ObligationAlertsResponse>> Handle(
        ListObligationAlertsQuery request,
        CancellationToken cancellationToken)
    {
        IEnumerable<ObligationAlert> alerts;

        var staff = await courtStaffRepository.GetByIdAsync(request.StaffId, cancellationToken);

        if (staff is null)
        {
            return Result.Failure<ObligationAlertsResponse>(
                CourtStaffErrors.NotFound(request.StaffId));
        }

        alerts = context.ObligationAlerts
            .Where(alert => alert.CourtId == staff.CourtId);

        if (!string.IsNullOrWhiteSpace(request.Status))
            alerts = alerts.Where(a => a.Status == request.Status.ToEnum<AlertStatus>());

        if (!string.IsNullOrWhiteSpace(request.Type))
            alerts = alerts.Where(a => a.Type == request.Type.ToEnum<AlertType>());

        _ = alerts.TryGetNonEnumeratedCount(out var totalCount);
        var pendingCount = alerts.Count(a => a.Status == AlertStatus.Pending);
        var underReviewCount = alerts.Count(a => a.Status == AlertStatus.UnderReview);
        var resolvedCount = alerts.Count(a => a.Status == AlertStatus.Resolved);

        var pagedAlerts = await alerts
            .OrderByDescending(a => a.TriggeredAt)
            .Select(alert => new ObligationAlertResponse(
                alert.Id,
                alert.CourtId,
                alert.ParentId,
                alert.RelatedEntityId,
                alert.Type.ToString(),
                alert.Description,
                alert.TriggeredAt,
                alert.Status.ToString(),
                alert.ResolvedAt,
                alert.ResolutionNotes))
            .Paginate(request.Pagination)
            .ToPagedResponseAsync(request.Pagination, totalCount);

        return new ObligationAlertsResponse(
            pagedAlerts,
            pendingCount,
            underReviewCount,
            resolvedCount);
    }
}