using Wesal.Application.Data;
using Wesal.Domain.Entities.CourtCases;

namespace Wesal.Application.Abstractions.Repositories;

public interface ICourtCaseRepository : IRepository<CourtCase>
{
    Task<bool> ExistsByCaseNumberAsync(string caseNumber, CancellationToken cancellationToken = default);
    Task<bool> HasOpenCaseByFamilyIdAsync(Guid familyId, CancellationToken cancellationToken = default);
}