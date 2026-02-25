using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.PaymentsDue;

namespace Wesal.Application.PaymentsDue.ListPaymentsDueByAlimony;

public record struct ListPaymentsDueByAlimonyQuery(
    Guid UserId,
    string UserRole,
    Guid AlimonyId,
    Pagination Pagination) : IQuery<PagedResponse<PaymentDueResponse>>;