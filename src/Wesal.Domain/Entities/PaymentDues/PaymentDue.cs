using Wesal.Domain.Common;
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
    public bool IsDueDatePassed => DueDate <= DateOnly.FromDateTime(DateTime.UtcNow);

    public static PaymentDue Create(Alimony alimony, DateOnly dueDate)
    {
        return new PaymentDue()
        {
            AlimonyId = alimony.Id,
            FamilyId = alimony.FamilyId,
            DueDate = dueDate,
            Amount = alimony.Amount,
            Status = PaymentStatus.Pending
        };
    }

    public Result MarkAsPaid(Guid paymentId)
    {
        var result = StatusTransition
            .Validate(Status, PaymentStatus.Pending, PaymentStatus.Paid);

        if (result.IsFailure)
            return result.Error;

        PaymentId = paymentId;
        PaidAt = DateTime.UtcNow;
        return Result.Success;
    }

    public Result MarkAsOverdue()
    {
        var result = StatusTransition
            .Validate(Status, PaymentStatus.Pending, PaymentStatus.Overdue);

        if (result.IsFailure)
            return result.Error;

        Status = PaymentStatus.Overdue;
        return Result.Success;
    }
}