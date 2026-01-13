using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Alimonies;

namespace Wesal.Domain.Entities.Payments;

public sealed class Payment : Entity
{
    public Guid AlimonyId { get; private set; }
    public long Amount { get; private set; }

    public PaymentStatus Status { get; private set; }
    public PaymentMethod Method { get; private set; }

    public string ReceiptUrl { get; private set; } = null!;
    public DateTime? PaidAt { get; private set; }

    public Alimony Alimony { get; private set; } = null!;

    private Payment() { }

    public static Payment Create(
        Guid alimonyId,
        long amount,
        PaymentStatus status,
        PaymentMethod method,
        string receiptUrl,
        DateTime? paidAt = null)
    {
        return new Payment
        {
            AlimonyId = alimonyId,
            PaidAt = paidAt,
            Amount = amount,
            Status = status,
            Method = method,
            ReceiptUrl = receiptUrl,
        };
    }
}