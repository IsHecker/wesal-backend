using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.ObligationAlerts;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.ObligationAlerts.ListObligationAlerts;

internal sealed class ListObligationAlertsQueryHandler(
    ICourtStaffRepository staffRepository,
    IWesalDbContext context)
    : IQueryHandler<ListObligationAlertsQuery, ObligationAlertsResponse>
{
    public async Task<Result<ObligationAlertsResponse>> Handle(
        ListObligationAlertsQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<ObligationAlert> alerts;

        var staff = await staffRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (staff is null)
            return UserErrors.NotFound(request.UserId);

        alerts = context.ObligationAlerts
            .Where(alert => alert.CourtId == staff.CourtId && alert.Status != AlertStatus.Drafted);

        if (!string.IsNullOrWhiteSpace(request.Status))
            alerts = alerts.Where(a => a.Status == request.Status.ToEnum<AlertStatus>());

        if (!string.IsNullOrWhiteSpace(request.ViolationType))
            alerts = alerts.Where(a => a.ViolationType == request.ViolationType.ToEnum<ViolationType>());

        var totalCount = alerts.Count();
        var pendingCount = alerts.Count(a => a.Status == AlertStatus.Pending);
        var underReviewCount = alerts.Count(a => a.Status == AlertStatus.UnderReview);
        var resolvedCount = alerts.Count(a => a.Status == AlertStatus.Resolved);

        var pagedAlerts = await alerts
            .OrderByDescending(a => a.TriggeredAt)
            .Paginate(request.Pagination)
            .Select(alert => new ObligationAlertResponse(
                alert.Id,
                alert.CourtId,
                alert.ParentId,
                alert.RelatedEntityId,
                alert.ViolationType.ToString(),
                alert.Description,
                alert.TriggeredAt,
                alert.Status.ToString(),
                alert.ResolvedAt,
                alert.ResolutionNotes))
            .ToPagedResponseAsync(request.Pagination, totalCount);

        return new ObligationAlertsResponse(
            pagedAlerts,
            pendingCount,
            underReviewCount,
            resolvedCount);
    }
}