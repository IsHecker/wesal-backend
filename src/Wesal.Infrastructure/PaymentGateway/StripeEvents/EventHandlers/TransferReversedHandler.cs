using Stripe;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway.StripeEvents.EventHandlers;

[StripeEvent(EventTypes.TransferReversed)]
internal sealed class TransferReversedHandler(
    IPaymentDueRepository paymentDueRepository,
    IUnitOfWork unitOfWork,
    INotificationService notificationService) : StripeEventHandler<Transfer>
{
    protected override async Task<Result> HandleAsync(Transfer transfer)
    {
        var recipientId = transfer.Get(MetadataKeys.RecipientId);
        var paymentDueId = transfer.Get(MetadataKeys.PaymentDueId);

        var paymentDue = await paymentDueRepository.GetByIdAsync(paymentDueId);

        paymentDue!.MarkWithdrawalStatusAs(WithdrawalStatus.Reversed);
        paymentDueRepository.Update(paymentDue);

        await unitOfWork.SaveChangesAsync();

        await notificationService.SendNotificationsAsync([
            NotificationTemplate.AlimonyWithdrawalFailed(recipientId, paymentDue.Amount)]);

        return Result.Success;
    }
}