using FluentValidation;

namespace Wesal.Application.PaymentsDue.InitiateAlimonyPayment;

internal sealed class InitiateAlimonyPaymentCommandValidator : AbstractValidator<InitiateAlimonyPaymentCommand>
{
    public InitiateAlimonyPaymentCommandValidator()
    {
        RuleFor(x => x.ParentId)
            .NotEmpty()
            .WithMessage("Parent ID is required");
    }
}