using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Alimonies;

using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.PaymentsDue;

public sealed class PaymentDue : Entity
{
    public Guid AlimonyId { get; private set; }
    public Guid FamilyId { get; private set; }
    public string? PaymentIntentId { get; private set; }
    public string? ReceiptUrl { get; private set; }

    public DateOnly DueDate { get; private set; }

    public long Amount { get; private set; }
    public PaymentStatus Status { get; private set; }
    public DateTime? PaidAt { get; private set; }

    public WithdrawalStatus? WithdrawalStatus { get; private set; }
    public DateTime? WithdrawnAt { get; private set; } = null!;

    public bool IsNotified { get; private set; }

    public bool IsPaid => PaidAt != null && Status == PaymentStatus.Paid;
    public bool IsDueDatePassed => DueDate < EgyptTime.Today;

    public Alimony Alimony { get; init; } = null!;

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

    public Result MarkAsPaid(string paymentIntentId, string? receiptUrl)
    {
        var result = StatusTransition
            .Validate(Status, PaymentStatus.Pending, PaymentStatus.Paid);

        if (result.IsFailure)
            return result.Error;

        PaymentIntentId = paymentIntentId;
        ReceiptUrl = receiptUrl;
        Status = PaymentStatus.Paid;
        PaidAt = EgyptTime.Now;

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

    public void MarkAsNotified() => IsNotified = true;

    public void MarkWithdrawalStatusAs(WithdrawalStatus status) => WithdrawalStatus = status;

    public void MarkAsWithdrawn()
    {
        WithdrawalStatus = PaymentsDue.WithdrawalStatus.Completed;
        WithdrawnAt = EgyptTime.Now;
    }
}