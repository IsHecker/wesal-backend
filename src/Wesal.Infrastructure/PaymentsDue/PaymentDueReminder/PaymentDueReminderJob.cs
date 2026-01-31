using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.PaymentsDue.PaymentDueReminder;

[DisallowConcurrentExecution]
internal sealed class PaymentDueReminderJob(
    IOptions<PaymentDueReminderOptions> options,
    INotificationService notificationService,
    WesalDbContext dbContext) : IJob
{
    private readonly PaymentDueReminderOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var upcomingPaymentsDue = await GenerateUpcomingPaymentsDueAsync(context.CancellationToken);

        foreach (var paymentDue in upcomingPaymentsDue)
        {
            await notificationService.SendNotificationAsync(
                NotificationTemplate.PaymentDue(paymentDue),
                new Dictionary<string, string>
                {
                    ["paymentDueId"] = paymentDue.Id.ToString()
                },
                context.CancellationToken);
        }
    }

    private async Task<List<PaymentDue>> GenerateUpcomingPaymentsDueAsync(CancellationToken cancellationToken)
    {
        var reminderDate = DateTime.UtcNow.AddDays(options.ReminderDaysBeforeDueDate).ToDateOnly();

        return await dbContext.PaymentsDue
            .Include(due => due.Alimony)
            .Where(due => due.DueDate == reminderDate
                && due.Status == PaymentStatus.Pending)
            .ToListAsync(cancellationToken);
    }
}