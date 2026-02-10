using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.PaymentsDue;

internal sealed class PaymentDueRepository(WesalDbContext context)
    : Repository<PaymentDue>(context), IPaymentDueRepository
{
    public async Task DeleteUnpaidByCourtCaseIdAsync(
        Guid courtCaseId,
        CancellationToken cancellationToken = default)
    {
        await context.PaymentsDue
            .Where(due => due.Alimony.CourtCaseId == courtCaseId
                && due.Status == PaymentStatus.Pending).ExecuteDeleteAsync(cancellationToken);
    }
}