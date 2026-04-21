using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Services;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.PaymentsDue.PaymentsDueGeneration;

[DisallowConcurrentExecution]
internal sealed class GeneratePaymentsDueJob(
    IOptions<GeneratePaymentsDueOptions> options,
    INotificationService notificationService,
    WesalDbContext dbContext) : IJob
{
    private readonly GeneratePaymentsDueOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var alimonies = await GetAlimoniesAsync(options.BatchSize, context.CancellationToken);

        foreach (var alimony in alimonies)
        {
            await GenerateUpcomingPaymentsDueAsync(alimony, context.CancellationToken);

            await dbContext.SaveChangesAsync(context.CancellationToken);

            await notificationService.SendNotificationsAsync(
                [NotificationTemplate.AlimoniesScheduled(alimony.PayerId)],
                cancellationToken: context.CancellationToken);
        }
    }

    private async Task<List<Alimony>> GetAlimoniesAsync(int batchSize, CancellationToken cancellationToken)
    {
        return await dbContext.Alimonies
            .Where(alimony => (!alimony.LastGeneratedDate.HasValue
                || alimony.LastGeneratedDate.Value <= EgyptTime.Today)
                && !alimony.IsStopped)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    private async Task GenerateUpcomingPaymentsDueAsync(Alimony alimony, CancellationToken cancellationToken)
    {
        var startDate = alimony.LastGeneratedDate ?? alimony.StartDate;

        var nextDates = ScheduleDateGenerator.GetNextDates(startDate, alimony.Frequency);

        foreach (var dueAt in nextDates)
        {
            var paymentDue = PaymentDue.Create(alimony, dueAt);
            alimony.UpdateLastGeneratedDate(dueAt);

            await dbContext.PaymentsDue.AddAsync(paymentDue, cancellationToken);
        }

        dbContext.Alimonies.Update(alimony);
    }
}