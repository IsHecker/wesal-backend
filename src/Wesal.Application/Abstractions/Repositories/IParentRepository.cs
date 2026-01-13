using Wesal.Application.Data;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Application.Abstractions.Repositories;

public interface IParentRepository : IRepository<Parent>
{
    Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken cancellationToken = default);
}