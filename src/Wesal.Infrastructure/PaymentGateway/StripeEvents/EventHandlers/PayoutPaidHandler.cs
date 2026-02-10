using Stripe;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway.StripeEvents.EventHandlers;

[StripeEvent(EventTypes.PayoutPaid)]
internal sealed class PayoutPaidHandler(
    IPaymentDueRepository paymentDueRepository,
    IUnitOfWork unitOfWork,
    INotificationService notificationService) : StripeEventHandler<Payout>
{
    protected override async Task<Result> HandleAsync(Payout payout)
    {
        // TODO: fix metadata population problem.
        var recipientId = payout.Get(MetadataKeys.RecipientId);
        var paymentDueId = payout.Get(MetadataKeys.PaymentDueId);

        var paymentDue = await paymentDueRepository.GetByIdAsync(paymentDueId);

        paymentDue!.MarkAsWithdrawn();
        paymentDueRepository.Update(paymentDue);

        await unitOfWork.SaveChangesAsync();

        await notificationService.SendNotificationsAsync([
            NotificationTemplate.AlimonyWithdrawalSuccess(recipientId, paymentDue.Amount)]);

        return Result.Success;
    }
}