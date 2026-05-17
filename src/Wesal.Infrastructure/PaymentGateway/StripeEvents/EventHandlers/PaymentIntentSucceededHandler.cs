using Stripe;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Notifications;

using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway.StripeEvents.EventHandlers;

[StripeEvent(EventTypes.PaymentIntentSucceeded)]
internal sealed class PaymentIntentSucceededHandler(
    IAlimonyRepository alimonyRepository,
    IPaymentDueRepository paymentDueRepository,
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

        paymentDue.MarkAsPaid(paymentIntent.Id, receiptUrl);

        paymentDueRepository.Update(paymentDue);

        await unitOfWork.SaveChangesAsync();

        await SendNotificationsAsync(alimony);

        return Result.Success;
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