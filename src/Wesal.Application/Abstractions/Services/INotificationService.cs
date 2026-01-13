namespace Wesal.Application.Abstractions.Services;

public interface INotificationService
{
    Task SendPaymentConfirmationAsync(
        Guid payerId,
        Guid receiverId,
        long amount,
        string currency,
        string receiptUrl,
        CancellationToken cancellationToken = default);
}