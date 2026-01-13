using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Payments;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Payments;

internal sealed class PaymentRepository(WesalDbContext context)
    : Repository<Payment>(context), IPaymentRepository
{
}