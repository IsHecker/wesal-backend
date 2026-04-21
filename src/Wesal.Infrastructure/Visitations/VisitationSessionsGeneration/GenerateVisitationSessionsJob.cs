using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Services;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Visitations.VisitationSessionsGeneration;

[DisallowConcurrentExecution]
internal sealed class GenerateVisitationSessionsJob(
    IOptions<GenerateVisitationSessionsOptions> options,
    INotificationService notificationService,
    WesalDbContext dbContext) : IJob
{
    private readonly GenerateVisitationSessionsOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var schedules = await GetSchedulesAsync(options.BatchSize, context.CancellationToken);

        foreach (var schedule in schedules)
        {
            await GenerateUpcomingVisitationsAsync(schedule, context.CancellationToken);
            await dbContext.SaveChangesAsync(context.CancellationToken);

            await notificationService.SendNotificationsAsync(
            [
                NotificationTemplate.VisitationScheduled(schedule.CustodialParentId),
                NotificationTemplate.VisitationScheduled(schedule.NonCustodialParentId)
            ], cancellationToken: context.CancellationToken);
        }
    }

    private async Task<List<VisitationSchedule>> GetSchedulesAsync(int batchSize, CancellationToken cancellationToken)
    {
        return await dbContext.VisitationSchedules
            .Where(schedule => (!schedule.LastGeneratedDate.HasValue
                || schedule.LastGeneratedDate.Value <= EgyptTime.Today)
                && !schedule.IsStopped)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    private async Task GenerateUpcomingVisitationsAsync(VisitationSchedule schedule, CancellationToken cancellationToken)
    {
        var startDate = schedule.LastGeneratedDate ?? schedule.StartDate;

        var nextDates = ScheduleDateGenerator.GetNextDates(startDate, schedule.Frequency);

        foreach (var visitationDate in nextDates)
        {
            var visitation = Visitation.Create(schedule, visitationDate.ToDateTime(schedule.StartTime));
            schedule.UpdateLastGeneratedDate(visitationDate);

            await dbContext.Visitations.AddAsync(visitation, cancellationToken);
        }

        dbContext.VisitationSchedules.Update(schedule);
    }
}