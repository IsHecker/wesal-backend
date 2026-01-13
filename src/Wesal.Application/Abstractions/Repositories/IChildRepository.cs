using Wesal.Application.Data;
using Wesal.Domain.Entities.Children;

namespace Wesal.Application.Abstractions.Repositories;

public interface IChildRepository : IRepository<Child>
{
    Task<List<Child>> GetAllByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken = default);
}