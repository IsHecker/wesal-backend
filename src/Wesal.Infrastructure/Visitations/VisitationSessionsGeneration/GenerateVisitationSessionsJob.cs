using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Visitations.VisitationSessionsGeneration;

[DisallowConcurrentExecution]
internal sealed class GenerateVisitationSessionsJob(
    IOptions<GenerateVisitationSessionsOptions> options,
    WesalDbContext dbContext) : IJob
{
    private readonly GenerateVisitationSessionsOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTime.UtcNow;
        var currentMonth = new DateOnly(now.Year, now.Month, 1);

        // Reads all active VisitationSchedule records
        var schedules = await GetSchedulesAsync(currentMonth, options.BatchSize);

        // System calculates next month's visits based on frequency
        // Creates Visitation records if they don't exist
        // System saves all **Visitation** sessions to database
        foreach (var schedule in schedules)
        {
            await GenerateUpcomingVisitationsAsync(schedule);
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private async Task<List<VisitationSchedule>> GetSchedulesAsync(DateOnly currentMonth, int batchSize)
    {
        return await dbContext.VisitationSchedules
            .Where(schedule => !schedule.LastGeneratedDate.HasValue
                || schedule.LastGeneratedDate.Value < currentMonth)
            .Take(batchSize)
            .ToListAsync();
    }

    private async Task GenerateUpcomingVisitationsAsync(VisitationSchedule schedule)
    {
        foreach (var visitationDate in GetNextVisitationDates(schedule))
        {
            var visitation = Visitation.Create(schedule, visitationDate);
            await dbContext.Visitations.AddAsync(visitation);

            schedule.UpdateLastGeneratedDate(visitationDate);
        }

        dbContext.VisitationSchedules.Update(schedule);
    }

    private static IEnumerable<DateOnly> GetNextVisitationDates(VisitationSchedule schedule)
    {
        var lastDayInMonth = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month);

        var frequencyDays = schedule.GetFrequencyInDays();
        var now = DateTime.UtcNow;

        for (int day = schedule.StartDayInMonth; day <= lastDayInMonth; day += frequencyDays)
        {
            yield return new DateOnly(now.Year, now.Month, day);
        }
    }
}