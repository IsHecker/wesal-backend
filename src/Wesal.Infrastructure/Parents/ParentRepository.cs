using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Parents;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Parents;

internal sealed class ParentRepository(WesalDbContext context)
    : Repository<Parent>(context), IParentRepository
{
    public Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default)
    {
        return context.Parents.AnyAsync(parent => parent.NationalId == nationalId, cancellationToken);
    }

    public Task<Parent?> GetByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default)
    {
        return context.Parents.FirstOrDefaultAsync(parent => parent.NationalId == nationalId, cancellationToken);
    }

    public Task<Parent?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return context.Parents
            .FirstOrDefaultAsync(school => school.UserId == userId, cancellationToken);
    }

    public Task<Parent> GetByStripeAccountIdAsync(string accountId, CancellationToken cancellationToken = default)
    {
        return context.Parents
            .AsTracking()
            .FirstAsync(w => w.StripeConnectAccountId == accountId, cancellationToken);
    }
}