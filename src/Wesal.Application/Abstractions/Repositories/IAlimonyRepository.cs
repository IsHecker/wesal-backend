using Wesal.Application.Data;
using Wesal.Domain.Entities.Alimonies;

namespace Wesal.Application.Abstractions.Repositories;

public interface IAlimonyRepository : IRepository<Alimony>
{
    Task<bool> ExistsByCourtCaseIdAsync(Guid courtCaseId, CancellationToken cancellationToken = default);
    Task<Alimony?> GetByCourtCaseIdAsync(Guid courtCaseId, CancellationToken cancellationToken = default);
}