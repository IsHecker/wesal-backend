using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Payments;

public static class WithdrawalErrors
{
    public static readonly Error Unauthorized =
        Error.Forbidden(
            "Withdrawal.Unauthorized",
            "You are not authorized to withdraw this payment. Only the recipient can withdraw.");

    public static readonly Error PaymentNotPaid =
        Error.Validation(
            "Withdrawal.PaymentNotPaid",
            "Cannot withdraw from a payment that has not been paid yet.");

    public static readonly Error AlreadyWithdrawn =
        Error.Validation(
            "Withdrawal.AlreadyWithdrawn",
            "This payment has already been withdrawn.");
}