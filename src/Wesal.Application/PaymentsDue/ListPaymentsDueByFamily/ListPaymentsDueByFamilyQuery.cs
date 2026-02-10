using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.PaymentsDue;

namespace Wesal.Application.PaymentsDue.ListPaymentsDueByFamily;

public record struct ListPaymentsDueByFamilyQuery(
    Guid UserId,
    string UserRole,
    Guid FamilyId,
    Pagination Pagination) : IQuery<PagedResponse<PaymentDueResponse>>;