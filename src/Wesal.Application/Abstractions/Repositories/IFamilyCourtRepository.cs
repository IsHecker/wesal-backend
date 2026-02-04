using Wesal.Application.Data;
using Wesal.Domain.Entities.FamilyCourts;

namespace Wesal.Application.Abstractions.Repositories;

public interface IFamilyCourtRepository : IRepository<FamilyCourt>
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}