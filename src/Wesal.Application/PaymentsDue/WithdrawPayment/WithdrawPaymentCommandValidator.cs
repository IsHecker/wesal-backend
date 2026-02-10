using FluentValidation;

namespace Wesal.Application.PaymentsDue.WithdrawPayment;

internal sealed class WithdrawPaymentCommandValidator
    : AbstractValidator<WithdrawPaymentCommand>
{
    public WithdrawPaymentCommandValidator()
    {
        RuleFor(x => x.ParentId)
            .NotEmpty()
            .WithMessage("Parent ID is required.");

        RuleFor(x => x.PaymentDueId)
            .NotEmpty()
            .WithMessage("Payment Due ID is required.");

        RuleFor(x => x.WithdrawalMethod)
            .NotEmpty()
            .WithMessage("Withdrawal method is required.");
        // .Must(BeValidWithdrawalMethod)
        // .WithMessage("Invalid withdrawal method. Valid options: BankTransfer, MobileWallet, Cash.");
    }
}