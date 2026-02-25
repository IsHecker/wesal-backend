using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.ObligationAlerts;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Results;

namespace Wesal.Application.ObligationAlerts.ListObligationAlerts;

internal sealed class ListObligationAlertsQueryHandler(IWesalDbContext context)
    : IQueryHandler<ListObligationAlertsQuery, ObligationAlertsResponse>
{
    public async Task<Result<ObligationAlertsResponse>> Handle(
        ListObligationAlertsQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<ObligationAlert> alerts;

        alerts = context.ObligationAlerts
            .Where(alert => alert.CourtId == request.CourtId && alert.Status != AlertStatus.Drafted);

        var pendingCount = alerts.Count(a => a.Status == AlertStatus.Pending);
        var underReviewCount = alerts.Count(a => a.Status == AlertStatus.UnderReview);
        var resolvedCount = alerts.Count(a => a.Status == AlertStatus.Resolved);

        if (!string.IsNullOrWhiteSpace(request.Status))
            alerts = alerts.Where(a => a.Status == request.Status.ToEnum<AlertStatus>());

        if (!string.IsNullOrWhiteSpace(request.ViolationType))
            alerts = alerts.Where(a => a.ViolationType == request.ViolationType.ToEnum<ViolationType>());

        var totalCount = alerts.Count();

        var pagedAlerts = await alerts
            .OrderByDescending(a => a.TriggeredAt)
            .Paginate(request.Pagination)
            .Select(alert => new ObligationAlertResponse(
                alert.Id,
                alert.CourtId,
                alert.ParentId,
                alert.RelatedEntityId,
                alert.ParentName,
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