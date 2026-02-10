using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Payments;

public sealed class Payment : Entity
{
    public Guid PaymentDueId { get; private set; }

    public PaymentStatus Status { get; private set; }
    public PaymentMethod Method { get; private set; }

    public string? PaymentIntentId { get; private set; }
    public string? ReceiptUrl { get; private set; }

    public DateTime PaidAt { get; private set; }

    private Payment() { }

    public static Payment Create(
        Guid paymentDueId,
        PaymentStatus status,
        string? paymentIntentId,
        string? receiptUrl,
        DateTime paidAt)
    {
        return new Payment
        {
            PaymentDueId = paymentDueId,
            Status = status,
            Method = PaymentMethod.CreditCard,
            PaymentIntentId = paymentIntentId,
            ReceiptUrl = receiptUrl,
            PaidAt = paidAt
        };
    }
}