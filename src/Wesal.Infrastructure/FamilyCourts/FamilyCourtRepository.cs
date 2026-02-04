using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.FamilyCourts;

internal sealed class FamilyCourtRepository(WesalDbContext context)
    : Repository<FamilyCourt>(context), IFamilyCourtRepository
{
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return context.FamilyCourts
            .AnyAsync(court => court.Email == email, cancellationToken);
    }
}