using Wesal.Application.Data;
using Wesal.Domain.Entities.Compliances;

namespace Wesal.Application.Abstractions.Repositories;

public interface IComplianceMetricsRepository : IRepository<ComplianceMetric>
{
    Task<ComplianceMetric?> GetAsync(
        Guid familyId,
        Guid parentId,
        DateOnly date,
        CancellationToken cancellationToken = default);
}