using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Schools;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Schools;

internal sealed class SchoolRepository(WesalDbContext context)
    : Repository<School>(context), ISchoolRepository
{
    public async Task<bool> ExistsByNameAndGovernorateAsync(
        string name,
        string governorate,
        CancellationToken cancellationToken = default)
    {
        return await context.Schools
            .AnyAsync(school => school.Name == name && school.Governorate == governorate, cancellationToken);
    }

    public Task<School?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return context.Schools
            .FirstOrDefaultAsync(school => school.UserId == userId, cancellationToken);
    }
}