using Wesal.Application.Data;
using Wesal.Domain.Entities.Custodies;

namespace Wesal.Application.Abstractions.Repositories;

public interface ICustodyRepository : IRepository<Custody>
{
    Task<Custody?> GetByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCourtCaseIdAsync(Guid courtCaseId, CancellationToken cancellationToken = default);
}