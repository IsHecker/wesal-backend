using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Domain.Entities.Payments;
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
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }

    private Task<List<PaymentDue>> GetMissedPaymentDuesAsync(int batchSize)
    {
        return dbContext.PaymentsDue
            .Where(IsDueDatePassed)
            .Where(IsNotPaid)
            .Where(due => due.Status != PaymentStatus.Overdue)
            .Take(batchSize)
            .ToListAsync();
    }

    private async Task ProcessPaymentDueAsync(PaymentDue paymentDue)
    {
        var result = paymentDue.MarkAsOverdue();
        if (result.IsFailure)
            throw new InvalidOperationException();

        dbContext.PaymentsDue.Update(paymentDue);

        var alimony = await alimonyRepository.GetByIdAsync(paymentDue.AlimonyId)
            ?? throw new InvalidOperationException();

        await alertService.RecordViolationAsync(
            alimony.PayerId,
            AlertType.UnpaidAlimony,
            paymentDue.Id,
            $@"failed to pay the alimony amount due on 
            {paymentDue.DueDate}. The payment is now marked as overdue.");
    }

    private static readonly Expression<Func<PaymentDue, bool>> IsNotPaid =
        due => due.PaidAt == null
            || due.PaymentId == null
            || due.Status != PaymentStatus.Paid;

    private static readonly Expression<Func<PaymentDue, bool>> IsDueDatePassed =
        due => due.DueDate <= DateOnly.FromDateTime(DateTime.UtcNow);
}