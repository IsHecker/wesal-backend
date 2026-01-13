using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Custodies;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Custodies;

internal sealed class CustodyRepository(WesalDbContext context)
    : Repository<Custody>(context), ICustodyRepository
{
    public Task<Custody?> GetByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        return context.Custodies
            .FirstOrDefaultAsync(custody => custody.FamilyId == familyId, cancellationToken);
    }
}