using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Payments;

public static class PaymentErrors
{
    public static Error Unauthorized(string message) =>
        Error.Forbidden("Payment.Unauthorized", message);

    public static Error InvalidAmount(long provided, long expected) =>
        Error.Validation(
            "Payment.InvalidAmount",
            $"Payment amount {provided} does not match obligation amount {expected}");
}