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
            .Include(family => family.Father)
            .Include(family => family.Mother)
            .Include(family => family.Children)
            .FirstOrDefaultAsync(family => family.FatherId == parentId || family.MotherId == parentId, cancellationToken);
    }
}