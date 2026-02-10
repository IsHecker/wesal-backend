using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Payments;

public static class PaymentErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Payment.NotFound", $"Payment with ID '{id}' was not found");

    public static Error Unauthorized(string message) =>
        Error.Forbidden("Payment.Unauthorized", message);
}