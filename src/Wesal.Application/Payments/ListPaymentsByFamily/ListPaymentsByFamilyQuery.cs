using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Payments;

namespace Wesal.Application.Payments.ListPaymentsByFamily;

public record struct ListPaymentsByFamilyQuery(
    Guid FamilyId,
    Pagination Pagination) : IQuery<PagedResponse<PaymentResponse>>;