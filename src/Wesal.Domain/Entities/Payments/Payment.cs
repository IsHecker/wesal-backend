using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Payments;

public sealed class Payment : Entity
{
    public Guid AlimonyId { get; private set; }
    public Guid PaymentDueId { get; private set; }
    public long Amount { get; private set; }

    public PaymentStatus Status { get; private set; }
    public PaymentMethod Method { get; private set; }

    public string ReceiptUrl { get; private set; } = null!;
    public DateTime PaidAt { get; private set; }

    private Payment() { }

    public static Payment Create(
        Guid alimonyId,
        Guid paymentDueId,
        long amount,
        PaymentStatus status,
        PaymentMethod method,
        string receiptUrl,
        DateTime paidAt)
    {
        return new Payment
        {
            AlimonyId = alimonyId,
            PaymentDueId = paymentDueId,
            Amount = amount,
            Status = status,
            Method = method,
            ReceiptUrl = receiptUrl,
            PaidAt = paidAt,
        };
    }
}

// public class Payment : Entity
// {
//     public string OrderId { get; private set; } = null!;
//     public string Gateway { get; private set; } = null!;
//     public decimal Amount { get; private set; }
//     public string Currency { get; private set; } = null!;
//     public PaymentStatus Status { get; private set; }
//     public string? TransactionId { get; private set; }
//     public string? ErrorMessage { get; private set; }
//     public DateTime CreatedAt { get; private set; }
//     public DateTime? CompletedAt { get; private set; }

//     private Payment() { }

//     public static Payment Create(
//         string orderId,
//         string gateway,
//         decimal amount,
//         string currency)
//     {
//         return new Payment
//         {
//             Id = Guid.NewGuid(),
//             OrderId = orderId,
//             Gateway = gateway,
//             Amount = amount,
//             Currency = currency,
//             Status = PaymentStatus.Pending,
//             CreatedAt = DateTime.UtcNow
//         };
//     }

//     public void MarkAsCompleted(string transactionId)
//     {
//         Status = PaymentStatus.Completed;
//         TransactionId = transactionId;
//         CompletedAt = DateTime.UtcNow;
//         ErrorMessage = null;
//     }

//     public void MarkAsPending(string? transactionId)
//     {
//         Status = PaymentStatus.Pending;
//         TransactionId = transactionId;
//         ErrorMessage = null;
//     }

//     public void MarkAsFailed(string errorMessage)
//     {
//         Status = PaymentStatus.Failed;
//         ErrorMessage = errorMessage;
//     }

//     public void MarkAsRefunded()
//     {
//         if (Status != PaymentStatus.Completed)
//             throw new InvalidOperationException("Can only refund completed payments");

//         Status = PaymentStatus.Refunded;
//     }
// }