using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Services;
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
            var notification = NotificationTemplate.UpcomingVisitation(visitation);

            await notificationService.SendNotificationAsync(
                notification,
                new Dictionary<string, string>
                {
                    ["visitationId"] = visitation.Id.ToString()
                },
                context.CancellationToken);
        }
    }

    private async Task<List<Visitation>> GetUpcomingVisitationsAsync(CancellationToken cancellationToken)
    {
        var daysBefore = options.ReminderDaysBeforeVisitation;

        var reminderThreshold = DateTime.UtcNow.AddDays(daysBefore).Date;
        var reminderWindowEnd = reminderThreshold.AddDays(daysBefore + 1);

        return await dbContext.Visitations
            .Where(visitation => visitation.StartAt >= reminderThreshold
                && visitation.StartAt < reminderWindowEnd
                && visitation.Status == VisitationStatus.Scheduled)
            .ToListAsync(cancellationToken);
    }
}