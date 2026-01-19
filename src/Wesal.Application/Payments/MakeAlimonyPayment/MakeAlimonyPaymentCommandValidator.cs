using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.Payments;

namespace Wesal.Application.Payments.MakeAlimonyPayment;

internal sealed class MakeAlimonyPaymentCommandValidator : AbstractValidator<MakeAlimonyPaymentCommand>
{
    public MakeAlimonyPaymentCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Payer ID is required");

        RuleFor(x => x.AlimonyId)
            .NotEmpty()
            .WithMessage("Alimony ID is required");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Payment amount must be greater than zero");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Payment method is required")
            .MustBeEnumValue<MakeAlimonyPaymentCommand, PaymentMethod>();
    }
}