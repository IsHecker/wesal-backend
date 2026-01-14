using Wesal.Application.Abstractions.Services;

namespace Wesal.Infrastructure.Notifications;

internal sealed class NotificationService : INotificationService
{
    public Task SendPaymentConfirmationAsync(
        Guid payerId,
        Guid receiverId,
        long amount,
        string currency,
        string receiptUrl,
        CancellationToken cancellationToken = default)
    {
        // TODO: implement notification

        return Task.CompletedTask;
    }
}