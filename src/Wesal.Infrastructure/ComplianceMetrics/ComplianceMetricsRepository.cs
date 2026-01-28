using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Compliances;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.ComplianceMetrics;

internal sealed class ComplianceMetricsRepository(WesalDbContext context)
    : Repository<ComplianceMetric>(context), IComplianceMetricsRepository
{
    public Task<ComplianceMetric?> GetAsync(
        Guid familyId,
        Guid parentId,
        DateOnly date,
        CancellationToken cancellationToken = default)
    {
        date = new DateOnly(date.Year, date.Month, 1);

        return context.ComplianceMetrics
            .AsTracking()
            .FirstOrDefaultAsync(metric =>
                metric.FamilyId == familyId
                && metric.ParentId == parentId
                && metric.Date == date, cancellationToken);
    }
}