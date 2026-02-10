using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Compliances;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.ComplianceMetrics;

internal sealed class ComplianceMetricsService(
    IComplianceMetricsRepository metricsRepository,
    WesalDbContext dbContext)
{
    public async Task<ComplianceMetric> GetOrCreateMetricsAsync(
        Guid courtId,
        Guid familyId,
        Guid parentId,
        DateOnly targetDate,
        CancellationToken cancellationToken = default)
    {
        var existing = await metricsRepository.GetAsync(
            familyId,
            parentId,
            targetDate,
            cancellationToken);

        if (existing is not null)
            return existing;

        var metrics = ComplianceMetric.Create(
            courtId,
            familyId,
            parentId,
            targetDate);

        await dbContext.ComplianceMetrics.AddAsync(metrics, cancellationToken);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return metrics;
        }
        catch (DbUpdateException)
        {
            dbContext.Entry(metrics).State = EntityState.Detached;

            return (await metricsRepository.GetAsync(
                familyId,
                parentId,
                targetDate,
                cancellationToken))!;
        }
    }
}