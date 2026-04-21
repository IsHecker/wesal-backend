using Wesal.Application.Data;
using Wesal.Domain.Entities.Payments;

namespace Wesal.Application.Abstractions.Repositories;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<Payment?> GetByPaymentDueIdAsync(Guid paymentDueId, CancellationToken cancellationToken = default);
}