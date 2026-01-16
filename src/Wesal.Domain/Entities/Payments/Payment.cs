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