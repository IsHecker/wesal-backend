using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.PaymentDues;

internal sealed class PaymentDueRepository(WesalDbContext context)
    : Repository<PaymentDue>(context), IPaymentDueRepository
{
}