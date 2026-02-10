using Stripe;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway.StripeEvents.EventHandlers;

[StripeEvent(EventTypes.PaymentIntentPaymentFailed)]
internal sealed class PaymentIntentPaymentFailedHandler(
    IPaymentDueRepository paymentDueRepository,
    IRepository<Payment> paymentRepository,
    IUnitOfWork unitOfWork,
    INotificationService notificationService) : StripeEventHandler<PaymentIntent>
{
    protected override async Task<Result> HandleAsync(PaymentIntent paymentIntent)
    {
        var payerId = paymentIntent.Get(MetadataKeys.PayerId);
        var paymentDueId = paymentIntent.Get(MetadataKeys.PaymentDueId);

        var paymentDue = await paymentDueRepository.GetByIdAsync(paymentDueId)
            ?? throw new InvalidOperationException();

        var payment = Payment.Create(
            paymentDue.Id,
            PaymentStatus.Failed,
            paymentIntent.Id,
            paymentIntent.LatestCharge.ReceiptUrl,
            EgyptTime.Now);

        await paymentRepository.AddAsync(payment);
        await unitOfWork.SaveChangesAsync();

        await notificationService
            .SendNotificationsAsync([NotificationTemplate.PaymentFailed(payerId, paymentDue.Amount)]);

        return Result.Success;
    }
}