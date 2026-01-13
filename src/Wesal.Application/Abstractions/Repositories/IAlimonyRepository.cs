using Wesal.Application.Data;
using Wesal.Domain.Entities.Alimonies;

namespace Wesal.Application.Abstractions.Repositories;

public interface IAlimonyRepository : IRepository<Alimony>
{
    Task<Alimony?> GetByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken = default);
}