using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.PaymentDues.PaymentDuesGeneration;

[DisallowConcurrentExecution]
internal sealed class GeneratePaymentDuesJob(
    IOptions<GeneratePaymentDuesOptions> options,
    WesalDbContext dbContext) : IJob
{
    private readonly GeneratePaymentDuesOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var now = DateTime.UtcNow;
        var currentMonth = new DateOnly(now.Year, now.Month, 1);

        var schedules = await GetAlimoniesAsync(currentMonth, options.BatchSize);

        foreach (var schedule in schedules)
        {
            await GenerateUpcomingPaymentsDueAsync(schedule);
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private async Task<List<Alimony>> GetAlimoniesAsync(DateOnly currentMonth, int batchSize)
    {
        return await dbContext.Alimonies
            .Where(alimony => !alimony.LastGeneratedDate.HasValue
                || alimony.LastGeneratedDate.Value < currentMonth)
            .Take(batchSize)
            .ToListAsync();
    }

    private async Task GenerateUpcomingPaymentsDueAsync(Alimony alimony)
    {
        foreach (var dueAt in GetNextVisitationDates(alimony))
        {
            var paymentDue = PaymentDue.Create(alimony, dueAt);
            await dbContext.PaymentsDue.AddAsync(paymentDue);

            alimony.UpdateLastGeneratedDate(dueAt);
        }

        dbContext.Alimonies.Update(alimony);
    }

    private static IEnumerable<DateOnly> GetNextVisitationDates(Alimony alimony)
    {
        var lastDayInMonth = DateTime.DaysInMonth(DateTime.UtcNow.Year, DateTime.UtcNow.Month);

        var frequencyDays = alimony.GetFrequencyInDays();
        var now = DateTime.UtcNow;

        for (int day = alimony.StartDayInMonth; day <= lastDayInMonth; day += frequencyDays)
        {
            yield return new DateOnly(now.Year, now.Month, day);
        }
    }
}