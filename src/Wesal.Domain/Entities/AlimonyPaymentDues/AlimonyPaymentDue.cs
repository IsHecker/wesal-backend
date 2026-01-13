using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.AlimonyPaymentDues;

public sealed class AlimonyPaymentDue : Entity
{
    public Guid AlimonyId { get; private set; }

    public DateTime DueDate { get; private set; }

    public long Amount { get; private set; }
    public AlimonyPaymentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public Guid? PaymentId { get; private set; }

    public static AlimonyPaymentDue Create(
        Guid alimonyId,
        DateTime dueDate,
        long amount,
        AlimonyPaymentStatus status)
    {
        return new AlimonyPaymentDue()
        {
            AlimonyId = alimonyId,
            DueDate = dueDate,
            Amount = amount,
            Status = status
        };
    }
}