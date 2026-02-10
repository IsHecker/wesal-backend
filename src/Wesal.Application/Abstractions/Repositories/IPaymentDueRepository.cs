using Wesal.Application.Data;
using Wesal.Domain.Entities.PaymentsDue;

namespace Wesal.Application.Abstractions.Repositories;

public interface IPaymentDueRepository : IRepository<PaymentDue>
{
    Task DeleteUnpaidByCourtCaseIdAsync(
        Guid courtCaseId,
        CancellationToken cancellationToken = default);
}