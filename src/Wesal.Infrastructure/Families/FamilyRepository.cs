using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Families;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Families;

internal sealed class FamilyRepository(WesalDbContext context)
    : Repository<Family>(context), IFamilyRepository
{
}