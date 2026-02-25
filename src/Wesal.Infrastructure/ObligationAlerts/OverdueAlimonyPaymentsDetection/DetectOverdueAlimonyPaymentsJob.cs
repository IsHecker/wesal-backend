using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.ObligationAlerts.OverdueAlimonyPaymentsDetection;

[DisallowConcurrentExecution]
internal sealed class DetectOverdueAlimonyPaymentsJob(
    IOptions<DetectOverdueAlimonyPaymentsOptions> options,
    IAlimonyRepository alimonyRepository,
    IComplianceMetricsRepository metricsRepository,
    ObligationAlertService alertService,
    WesalDbContext dbContext) : IJob
{
    private readonly DetectOverdueAlimonyPaymentsOptions options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        var paymentsDue = await GetMissedPaymentsDueAsync(options.BatchSize, context.CancellationToken);

        foreach (var paymentDue in paymentsDue)
        {
            await ProcessPaymentDueAsync(paymentDue, context.CancellationToken);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }

    private Task<List<PaymentDue>> GetMissedPaymentsDueAsync(int batchSize, CancellationToken cancellationToken)
    {
        return dbContext.PaymentsDue
            .Where(IsDueDatePassed)
            .Where(IsNotPaid)
            .Where(due => due.Status != PaymentStatus.Overdue)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    private async Task ProcessPaymentDueAsync(PaymentDue paymentDue, CancellationToken cancellationToken)
    {
        var result = paymentDue.MarkAsOverdue();
        if (result.IsFailure)
            throw new InvalidOperationException();

        dbContext.PaymentsDue.Update(paymentDue);

        var alimony = await alimonyRepository.GetByIdAsync(paymentDue.AlimonyId, cancellationToken)
            ?? throw new InvalidOperationException();

        await alertService.RecordViolationAsync(
            alimony.PayerId,
            ViolationType.UnpaidAlimony,
            paymentDue.Id,
            $@"Failed to pay the alimony amount due on 
            {paymentDue.DueDate}. The payment is now marked as overdue.",
            cancellationToken);

        await RecordAlimonyOverdueAsync(alimony, cancellationToken);
    }

    private async Task RecordAlimonyOverdueAsync(Alimony alimony, CancellationToken cancellationToken)
    {
        var metrics = await metricsRepository.GetAsync(
            alimony.FamilyId,
            alimony.PayerId,
            EgyptTime.Today,
            cancellationToken) ?? throw new InvalidOperationException();

        metrics.RecordAlimonyOverdue();
    }

    private static readonly Expression<Func<PaymentDue, bool>> IsNotPaid =
        due => due.PaidAt == null
            || due.PaymentId == null
            || due.Status != PaymentStatus.Paid;

    private static readonly Expression<Func<PaymentDue, bool>> IsDueDatePassed =
        due => EgyptTime.Today > due.DueDate;
}