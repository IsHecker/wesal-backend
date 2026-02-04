using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Families;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Families;

internal sealed class FamilyRepository(WesalDbContext context)
    : Repository<Family>(context), IFamilyRepository
{
    public Task<Family?> GetByParentIdAsync(Guid parentId, CancellationToken cancellationToken = default)
    {
        return context.Families
            .FirstOrDefaultAsync(family => family.FatherId == parentId || family.MotherId == parentId, cancellationToken);
    }

    public Task<bool> IsParentInFamilyAsync(Guid parentId, Guid familyId, CancellationToken cancellationToken = default)
    {
        return context.Families
            .AnyAsync(family => family.Id == familyId && (family.FatherId == parentId || family.MotherId == parentId),
                cancellationToken);
    }
}