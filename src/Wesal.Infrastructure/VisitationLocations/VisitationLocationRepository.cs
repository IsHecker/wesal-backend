using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.VisitationLocations;

internal sealed class VisitationLocationRepository(WesalDbContext context)
    : Repository<VisitationLocation>(context), IVisitationLocationRepository
{
    public async Task<bool> ExistsByNameAndGovernorateAsync(
        string name,
        string governorate,
        CancellationToken cancellationToken = default)
    {
        return await context.VisitationLocations
            .AnyAsync(location => location.Name == name && location.Governorate == governorate, cancellationToken);
    }
}