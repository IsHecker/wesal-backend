using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.PaymentDues;

public sealed class PaymentDue : Entity
{
    public Guid AlimonyId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid? PaymentId { get; private set; }

    public DateOnly DueDate { get; private set; }

    public long Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }

    public bool IsPaid => PaidAt != null && PaymentId != null && Status == PaymentStatus.Paid;
    public bool IsDueDatePassed => DueDate >= DateOnly.FromDateTime(DateTime.UtcNow);

    public static PaymentDue Create(Alimony alimony, DateOnly dueDate)
    {
        return new PaymentDue()
        {
            AlimonyId = alimony.Id,
            DueDate = dueDate,
            Amount = alimony.Amount,
            Status = PaymentStatus.Pending
        };
    }

    public Result MarkAsPaid(Guid paymentId)
    {
        var result = ValidateTransition(PaymentStatus.Pending, PaymentStatus.Paid);
        if (result.IsFailure)
            return result.Error;

        PaymentId = paymentId;
        PaidAt = DateTime.UtcNow;
        return Result.Success;
    }

    public Result MarkAsOverdue()
    {
        return ValidateTransition(PaymentStatus.Pending, PaymentStatus.Overdue);
    }

    private Result ValidateTransition(
        PaymentStatus requiredStatus,
        PaymentStatus targetStatus)
    {
        if (Status == targetStatus)
            return PaymentDueErrors.IsAlready(Status);

        if (Status != requiredStatus)
            return PaymentDueErrors.CannotTransition(Status, targetStatus);

        return Result.Success;
    }
}