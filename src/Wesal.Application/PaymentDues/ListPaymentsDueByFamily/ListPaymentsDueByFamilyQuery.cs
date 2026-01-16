using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.PaymentDues;

namespace Wesal.Application.PaymentDues.ListPaymentsDueByFamily;

public record struct ListPaymentsDueByFamilyQuery(Guid FamilyId, Pagination Pagination)
    : IQuery<PagedResponse<PaymentDueResponse>>;