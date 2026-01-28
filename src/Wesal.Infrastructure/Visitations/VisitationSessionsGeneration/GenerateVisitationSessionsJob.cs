using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.Compliances;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Infrastructure.ComplianceMetrics;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Visitations.VisitationSessionsGeneration;

[DisallowConcurrentExecution]
internal sealed class GenerateVisitationSessionsJob(
    IOptions<GenerateVisitationSessionsOptions> options,
    IComplianceMetricsRepository metricsRepository,
    WesalDbContext dbContext) : IJob
{
    private readonly GenerateVisitationSessionsOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTime.UtcNow;
        var currentMonth = new DateOnly(now.Year, now.Month, 1);

        // Reads all active VisitationSchedule records
        var schedules = await GetSchedulesAsync(currentMonth, options.BatchSize, context.CancellationToken);

        // System calculates next month's visits based on frequency
        // Creates Visitation records if they don't exist
        // System saves all **Visitation** sessions to database
        foreach (var schedule in schedules)
        {
            await GenerateUpcomingVisitationsAsync(schedule, context.CancellationToken);
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private async Task<List<VisitationSchedule>> GetSchedulesAsync(
        DateOnly currentMonth,
        int batchSize,
        CancellationToken cancellationToken)
    {
        return await dbContext.VisitationSchedules
            .Where(schedule => !schedule.LastGeneratedDate.HasValue
                || schedule.LastGeneratedDate.Value < currentMonth)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    private async Task GenerateUpcomingVisitationsAsync(VisitationSchedule schedule, CancellationToken cancellationToken)
    {
        var targetDate = schedule.LastGeneratedDate.HasValue
            ? DateTime.UtcNow.ToDateOnly()
            : schedule.StartDate;

        // TODO: Find cleaner way to handle this creation!
        var metrics = await GetOrCreateMetricsAsync(
            schedule.CourtId,
            schedule.FamilyId,
            schedule.ParentId,
            DateTime.UtcNow.ToDateOnly(),
            cancellationToken);

        foreach (var visitationDate in GetNextVisitationDates(schedule, targetDate))
        {
            var visitation = Visitation.Create(schedule, visitationDate.ToDateTime(schedule.StartTime));
            schedule.UpdateLastGeneratedDate(visitationDate);
            metrics.RecordVisitationScheduled();

            await dbContext.Visitations.AddAsync(visitation, cancellationToken);
        }

        dbContext.VisitationSchedules.Update(schedule);
    }

    private static IEnumerable<DateOnly> GetNextVisitationDates(VisitationSchedule schedule, DateOnly targetDate)
    {
        var lastDayInMonth = DateTime.DaysInMonth(targetDate.Year, targetDate.Month);
        var startDayInMonth = schedule.StartDate.Day;
        var frequencyDays = schedule.GetFrequencyInDays(targetDate);

        for (int day = startDayInMonth; day <= lastDayInMonth; day += frequencyDays)
        {
            yield return new DateOnly(targetDate.Year, targetDate.Month, day);
        }
    }

    private async Task<ComplianceMetric> GetOrCreateMetricsAsync(
        Guid courtId,
        Guid familyId,
        Guid parentId,
        DateOnly targetDate,
        CancellationToken cancellationToken = default)
    {
        var metrics = await metricsRepository.GetAsync(familyId, parentId, targetDate, cancellationToken);

        if (metrics is null)
        {
            metrics = ComplianceMetric.Create(courtId, familyId, parentId, targetDate);
            await metricsRepository.AddAsync(metrics, cancellationToken);
        }

        return metrics;
    }
}