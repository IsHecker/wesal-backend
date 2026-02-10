using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Services;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Infrastructure.ComplianceMetrics;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Visitations.VisitationSessionsGeneration;

[DisallowConcurrentExecution]
internal sealed class GenerateVisitationSessionsJob(
    IOptions<GenerateVisitationSessionsOptions> options,
    ComplianceMetricsService metricsService,
    INotificationService notificationService,
    WesalDbContext dbContext) : IJob
{
    private readonly GenerateVisitationSessionsOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var now = EgyptTime.Now;
        var currentMonth = new DateOnly(now.Year, now.Month, 1);

        var schedules = await GetSchedulesAsync(currentMonth, options.BatchSize, context.CancellationToken);

        foreach (var schedule in schedules)
        {
            await GenerateUpcomingVisitationsAsync(schedule, context.CancellationToken);

            await notificationService.SendNotificationsAsync(
            [
                NotificationTemplate.VisitationScheduled(schedule.CustodialParentId),
                NotificationTemplate.VisitationScheduled(schedule.NonCustodialParentId)
            ], cancellationToken: context.CancellationToken);
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private async Task<List<VisitationSchedule>> GetSchedulesAsync(
        DateOnly currentMonth,
        int batchSize,
        CancellationToken cancellationToken)
    {
        return await dbContext.VisitationSchedules
            .Where(schedule => (!schedule.LastGeneratedDate.HasValue
                || schedule.LastGeneratedDate.Value < currentMonth)
                && !schedule.IsStopped)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    private async Task GenerateUpcomingVisitationsAsync(VisitationSchedule schedule, CancellationToken cancellationToken)
    {
        var targetDate = schedule.LastGeneratedDate.HasValue
            ? EgyptTime.Today
            : schedule.StartDate;

        var metrics = await metricsService.GetOrCreateMetricsAsync(
            schedule.CourtId,
            schedule.FamilyId,
            schedule.NonCustodialParentId,
            EgyptTime.Today,
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
}