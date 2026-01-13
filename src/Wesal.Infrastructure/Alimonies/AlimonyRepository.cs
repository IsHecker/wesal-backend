using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Alimonies;

internal sealed class AlimonyRepository(WesalDbContext context)
    : Repository<Alimony>(context), IAlimonyRepository
{
    public Task<Alimony?> GetByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        return context.Alimonies
            .FirstOrDefaultAsync(alimony => alimony.FamilyId == familyId, cancellationToken);
    }
}