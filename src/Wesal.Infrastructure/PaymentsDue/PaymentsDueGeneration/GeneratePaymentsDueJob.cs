using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Services;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Infrastructure.ComplianceMetrics;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.PaymentsDue.PaymentsDueGeneration;

[DisallowConcurrentExecution]
internal sealed class GeneratePaymentsDueJob(
    IOptions<GeneratePaymentsDueOptions> options,
    ComplianceMetricsService metricsService,
    INotificationService notificationService,
    WesalDbContext dbContext) : IJob
{
    private readonly GeneratePaymentsDueOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var now = EgyptTime.Now;
        var currentMonth = new DateOnly(now.Year, now.Month, 1);

        var alimonies = await GetAlimoniesAsync(currentMonth, options.BatchSize, context.CancellationToken);

        foreach (var alimony in alimonies)
        {
            await GenerateUpcomingPaymentsDueAsync(alimony, context.CancellationToken);

            await notificationService.SendNotificationsAsync(
                [NotificationTemplate.AlimoniesScheduled(alimony.PayerId)],
                cancellationToken: context.CancellationToken);
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private async Task<List<Alimony>> GetAlimoniesAsync(
        DateOnly currentMonth,
        int batchSize,
        CancellationToken cancellationToken)
    {
        return await dbContext.Alimonies
            .Where(alimony => (!alimony.LastGeneratedDate.HasValue
                || alimony.LastGeneratedDate.Value < currentMonth)
                && !alimony.IsStopped)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    private async Task GenerateUpcomingPaymentsDueAsync(Alimony alimony, CancellationToken cancellationToken)
    {
        var targetDate = alimony.LastGeneratedDate.HasValue
            ? EgyptTime.Today
            : alimony.StartDate;

        var metrics = await metricsService.GetOrCreateMetricsAsync(
            alimony.CourtId,
            alimony.FamilyId,
            alimony.PayerId,
            targetDate,
            cancellationToken);

        foreach (var dueAt in GetNextPaymentDueDates(alimony, targetDate))
        {
            var paymentDue = PaymentDue.Create(alimony, dueAt);
            alimony.UpdateLastGeneratedDate(dueAt);

            metrics.RecordAlimonyDue();

            await dbContext.PaymentsDue.AddAsync(paymentDue, cancellationToken);
        }

        dbContext.Alimonies.Update(alimony);
    }

    private static IEnumerable<DateOnly> GetNextPaymentDueDates(Alimony alimony, DateOnly targetDate)
    {
        var lastDayInMonth = DateTime.DaysInMonth(targetDate.Year, targetDate.Month);

        var startDayInMonth = alimony.StartDate.Day;
        var frequencyDays = alimony.GetFrequencyInDays(targetDate);

        for (int day = startDayInMonth; day <= lastDayInMonth; day += frequencyDays)
        {
            yield return new DateOnly(targetDate.Year, targetDate.Month, day);
        }
    }
}