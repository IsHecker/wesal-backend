using Wesal.Application.Data;
using Wesal.Domain.Entities.Families;

namespace Wesal.Application.Abstractions.Repositories;

public interface IFamilyRepository : IRepository<Family>
{
    Task<Family?> GetByParentIdAsync(Guid parentId, CancellationToken cancellationToken = default);
    Task<bool> IsParentInFamilyAsync(Guid parentId, Guid familyId, CancellationToken cancellationToken = default);
}