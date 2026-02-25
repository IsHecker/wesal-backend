using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.PaymentsDue;

public static class PaymentDueErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("PaymentDue.NotFound", $"Payment due with ID '{id}' was not found");

    public static Error IsAlreadyPaid =>
        Error.Validation("PaymentDue.IsAlreadyPaid", $"Payment due is already paid");

    public static Error DueDatePassed =>
        Error.Validation("PaymentDue.DueDatePassed", $"Due date has passed for this payment");

    public static Error IsAlready(PaymentStatus status) =>
        Error.Validation(
            $"PaymentDue.IsAlready{status}",
            $"This Payment due is already '{status}'");

    public static Error CannotTransition(PaymentStatus currentStatus, PaymentStatus targetStatus) =>
        Error.Validation(
            "PaymentDue.InvalidStatusTransition",
            $"Cannot change Payment due status from '{currentStatus}' to '{targetStatus}'.");
}