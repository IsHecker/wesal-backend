using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Payments;

public static class PaymentErrors
{
    public static Error Unauthorized(string message) =>
        Error.Forbidden("Payment.Unauthorized", message);
}