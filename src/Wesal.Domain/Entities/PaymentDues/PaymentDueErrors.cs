using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.PaymentDues;

public static class PaymentDueErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("PaymentDue.NotFound", $"Payment due with ID '{id}' was not found");

    public static Error IsAlreadyPaid =>
        Error.NotFound("PaymentDue.IsAlreadyPaid", $"Payment due is already paid");

    public static Error DueDatePassed =>
        Error.NotFound("PaymentDue.DueDatePassed", $"Due date has passed for this payment");
}