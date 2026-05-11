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
        await _context.PaymentsDue
            .Where(due => due.Alimony.CourtCaseId == courtCaseId
                && due.Status == PaymentStatus.Pending).ExecuteDeleteAsync(cancellationToken);
    }
    
    public async Task DeletePendingByAlimonyIdAsync(
        Guid alimonyId,
        CancellationToken cancellationToken = default)
    {
        await _context.PaymentsDue
            .Where(due => due.AlimonyId == alimonyId
                && due.Status == PaymentStatus.Pending).ExecuteDeleteAsync(cancellationToken);
    }

    public Task<bool> HasPaymentsByAlimonyIdAsync(
        Guid alimonyId,
        CancellationToken cancellationToken = default)
    {
        return _context.PaymentsDue
            .AnyAsync(due => due.AlimonyId == alimonyId
                && due.Status != PaymentStatus.Pending, cancellationToken);
    }
}