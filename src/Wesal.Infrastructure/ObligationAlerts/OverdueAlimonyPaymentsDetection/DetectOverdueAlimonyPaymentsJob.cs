using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.ObligationAlerts.OverdueAlimonyPaymentsDetection;

[DisallowConcurrentExecution]
internal sealed class DetectOverdueAlimonyPaymentsJob(
    IOptions<DetectOverdueAlimonyPaymentsOptions> options,
    IAlimonyRepository alimonyRepository,
    ObligationAlertService alertService,
    WesalDbContext dbContext) : IJob
{
    private readonly DetectOverdueAlimonyPaymentsOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var paymentDues = await GetMissedPaymentDuesAsync(options.BatchSize);

        foreach (var paymentDue in paymentDues)
        {
            await ProcessPaymentDueAsync(paymentDue);
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private Task<List<PaymentDue>> GetMissedPaymentDuesAsync(int batchSize)
    {
        return dbContext.PaymentsDue
            .Where(due => !due.IsPaid)
            .Take(batchSize)
            .ToListAsync();
    }

    private async Task ProcessPaymentDueAsync(PaymentDue paymentDue)
    {
        paymentDue.MarkAsOverdue();
        dbContext.PaymentsDue.Update(paymentDue);

        var alimony = await alimonyRepository.GetByIdAsync(paymentDue.AlimonyId)
            ?? throw new InvalidOperationException();

        await alertService.CreateAlertAsync(
            alimony.PayerId,
            AlertType.UnpaidAlimony,
            paymentDue.Id,
            $@"failed to pay the alimony amount due on 
            {paymentDue.DueDate}. The payment is now marked as overdue.");
    }
}