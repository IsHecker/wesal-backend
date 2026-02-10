using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Payments;

namespace Wesal.Application.Payments.ListPaymentsByPaymentDue;

public record struct ListPaymentsByPaymentDueQuery(Guid PaymentDueId, Pagination Pagination)
    : IQuery<PagedResponse<PaymentResponse>>;