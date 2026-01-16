namespace Wesal.Application.Abstractions.Services;

public interface INotificationService
{
    Task SendPaymentConfirmationAsync(
        Guid payerId,
        Guid recipientId,
        long amount,
        string receiptUrl,
        CancellationToken cancellationToken = default);
}