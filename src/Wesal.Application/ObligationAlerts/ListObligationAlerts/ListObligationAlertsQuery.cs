using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.ObligationAlerts;

namespace Wesal.Application.ObligationAlerts.ListObligationAlerts;

public record struct ListObligationAlertsQuery(
    Guid UserId,
    string? Status,
    string? Type,
    Pagination Pagination) : IQuery<ObligationAlertsResponse>;