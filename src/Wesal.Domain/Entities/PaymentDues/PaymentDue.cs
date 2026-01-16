using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Payments;

namespace Wesal.Domain.Entities.PaymentDues;

public sealed class PaymentDue : Entity
{
    public Guid AlimonyId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid? PaymentId { get; private set; }

    public DateTime DueDate { get; private set; }

    public long Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }

    public bool IsPaid => PaidAt != null && PaymentId != null;
    public bool IsDueDatePassed => DueDate >= DateTime.UtcNow;

    public static PaymentDue Create(
        Guid alimonyId,
        DateTime dueDate,
        long amount,
        PaymentStatus status)
    {
        return new PaymentDue()
        {
            AlimonyId = alimonyId,
            DueDate = dueDate,
            Amount = amount,
            Status = status
        };
    }

    public void MarkAsPaid(Guid paymentId)
    {
        PaymentId = paymentId;
        PaidAt = DateTime.UtcNow;
    }
}