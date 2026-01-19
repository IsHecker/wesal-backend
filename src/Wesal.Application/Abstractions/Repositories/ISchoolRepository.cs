using Wesal.Application.Data;
using Wesal.Domain.Entities.Schools;

namespace Wesal.Application.Abstractions.Repositories;

public interface ISchoolRepository : IRepository<School>
{
    Task<School?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAndGovernorateAsync(
        string name,
        string governorate,
        CancellationToken cancellationToken = default);
}