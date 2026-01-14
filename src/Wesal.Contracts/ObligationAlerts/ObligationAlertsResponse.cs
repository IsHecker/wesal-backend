using Wesal.Contracts.Common;

namespace Wesal.Contracts.ObligationAlerts;

public record struct ObligationAlertsResponse(
    PagedResponse<ObligationAlertResponse> Alerts,
    int PendingCount,
    int UnderReviewCount,
    int ResolvedCount);