using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Children;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Children;

internal sealed class ChildRepository(WesalDbContext context)
    : Repository<Child>(context), IChildRepository
{
    public Task<List<Child>> GetAllByFamilyIdAsync(
        Guid familyId,
        CancellationToken cancellationToken = default)
    {
        return context.Children
            .Where(child => child.FamilyId == familyId)
            .ToListAsync(cancellationToken);
    }
}