using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.PaymentsDue;

internal sealed class PaymentDueRepository(WesalDbContext context)
    : Repository<PaymentDue>(context), IPaymentDueRepository
{
}