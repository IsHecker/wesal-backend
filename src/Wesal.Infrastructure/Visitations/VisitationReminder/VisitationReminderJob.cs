using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Services;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Visitations;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Visitations.VisitationReminder;

[DisallowConcurrentExecution]
internal sealed class VisitationReminderJob(
    IOptions<VisitationReminderOptions> options,
    WesalDbContext dbContext,
    INotificationService notificationService) : IJob
{
    private readonly VisitationReminderOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var upcomingVisitations = await GetUpcomingVisitationsAsync(context.CancellationToken);

        foreach (var visitation in upcomingVisitations)
        {
            visitation.MarkAsNotified();

            var schedule = visitation.VisitationSchedule;

            await notificationService.SendNotificationsAsync(
                [
                    NotificationTemplate.UpcomingVisitation(schedule.CustodialParentId, visitation.StartAt),
                    NotificationTemplate.UpcomingVisitation(schedule.NonCustodialParentId, visitation.StartAt)
                ],
                new Dictionary<string, string>
                {
                    ["visitationId"] = visitation.Id.ToString()
                },
                context.CancellationToken);
        }
    }

    private async Task<List<Visitation>> GetUpcomingVisitationsAsync(CancellationToken cancellationToken)
    {
        var targetStart = EgyptTime.Now.AddDays(options.ReminderDaysBeforeVisitation);

        var from = targetStart.AddHours(-1);
        var to = targetStart.AddHours(1);

        return await dbContext.Visitations
            .AsTracking()
            .Include(v => v.VisitationSchedule)
            .Where(visitation => !visitation.IsNotified
                && from <= visitation.StartAt && visitation.StartAt <= to
                && visitation.Status == VisitationStatus.Scheduled)
            .ToListAsync(cancellationToken);
    }
}