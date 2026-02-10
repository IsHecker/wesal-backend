using Stripe;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway.StripeEvents.EventHandlers;

[StripeEvent(EventTypes.PaymentIntentSucceeded)]
internal sealed class PaymentIntentSucceededHandler(
    IAlimonyRepository alimonyRepository,
    IPaymentDueRepository paymentDueRepository,
    IRepository<Payment> paymentRepository,
    IComplianceMetricsRepository metricsRepository,
    IUnitOfWork unitOfWork,
    INotificationService notificationService) : StripeEventHandler<PaymentIntent>
{
    protected override async Task<Result> HandleAsync(PaymentIntent paymentIntent)
    {
        var alimonyId = paymentIntent.Get(MetadataKeys.AlimonyId);
        var paymentDueId = paymentIntent.Get(MetadataKeys.PaymentDueId);

        var alimony = await alimonyRepository.GetByIdAsync(alimonyId)
            ?? throw new InvalidOperationException();

        var paymentDue = await paymentDueRepository.GetByIdAsync(paymentDueId)
            ?? throw new InvalidOperationException();

        paymentIntent = await new PaymentIntentService().GetAsync(
            paymentIntent.Id,
            new PaymentIntentGetOptions
            {
                Expand = ["latest_charge"]
            });

        var receiptUrl = paymentIntent.LatestCharge.ReceiptUrl;

        var payment = Payment.Create(
            paymentDueId,
            PaymentStatus.Paid,
            paymentIntent.Id,
            receiptUrl,
            EgyptTime.Now);

        paymentDue.MarkAsPaid(payment.Id);

        await paymentRepository.AddAsync(payment);
        paymentDueRepository.Update(paymentDue);

        await RecordAlimonyPaidOnTimeAsync(alimony);

        await unitOfWork.SaveChangesAsync();

        await SendNotificationsAsync(alimony);

        return Result.Success;
    }

    private async Task RecordAlimonyPaidOnTimeAsync(Alimony alimony)
    {
        var metrics = await metricsRepository.GetAsync(
            alimony.FamilyId,
            alimony.PayerId,
            EgyptTime.Today) ?? throw new InvalidOperationException();

        metrics.RecordAlimonyPaidOnTime();
        metricsRepository.Update(metrics);
    }

    private async Task SendNotificationsAsync(Alimony alimony)
    {
        await notificationService.SendNotificationsAsync(
        [
            NotificationTemplate.PaymentSuccess(alimony)
        ]);

        await notificationService.SendNotificationsAsync(
        [
            NotificationTemplate.AlimonyReadyToWithdraw(alimony)
        ]);
    }
}