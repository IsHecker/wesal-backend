using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.PaymentDues.PaymentDuesGeneration;

[DisallowConcurrentExecution]
internal sealed class GeneratePaymentDuesJob(
    IOptions<GeneratePaymentDuesOptions> options,
    IComplianceMetricsRepository metricsRepository,
    WesalDbContext dbContext) : IJob
{
    private readonly GeneratePaymentDuesOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTime.UtcNow;
        var currentMonth = new DateOnly(now.Year, now.Month, 1);

        var schedules = await GetAlimoniesAsync(currentMonth, options.BatchSize, context.CancellationToken);

        foreach (var schedule in schedules)
        {
            await GenerateUpcomingPaymentsDueAsync(schedule, context.CancellationToken);
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private async Task<List<Alimony>> GetAlimoniesAsync(
        DateOnly currentMonth,
        int batchSize,
        CancellationToken cancellationToken)
    {
        return await dbContext.Alimonies
            .Where(alimony => !alimony.LastGeneratedDate.HasValue
                || alimony.LastGeneratedDate.Value < currentMonth)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    private async Task GenerateUpcomingPaymentsDueAsync(Alimony alimony, CancellationToken cancellationToken)
    {
        var targetDate = alimony.LastGeneratedDate.HasValue
            ? DateTime.UtcNow.ToDateOnly()
            : alimony.StartDate;

        var metrics = await metricsRepository.GetAsync(
            alimony.FamilyId,
            alimony.PayerId,
            targetDate,
            cancellationToken);

        if (metrics is null)
            return;

        foreach (var dueAt in GetNextVisitationDates(alimony, targetDate))
        {
            var paymentDue = PaymentDue.Create(alimony, dueAt);
            alimony.UpdateLastGeneratedDate(dueAt);

            metrics.RecordAlimonyDue();

            await dbContext.PaymentsDue.AddAsync(paymentDue, cancellationToken);
        }

        dbContext.Alimonies.Update(alimony);
    }

    private static IEnumerable<DateOnly> GetNextVisitationDates(Alimony alimony, DateOnly targetDate)
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