using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.Payments;

namespace Wesal.Application.Payments.MakeAlimonyPayment;

internal sealed class MakeAlimonyPaymentCommandValidator : AbstractValidator<MakeAlimonyPaymentCommand>
{
    public MakeAlimonyPaymentCommandValidator()
    {
        RuleFor(x => x.ParentId)
            .NotEmpty()
            .WithMessage("Parent ID is required");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty()
            .WithMessage("Payment method is required")
            .MustBeEnumValue<MakeAlimonyPaymentCommand, PaymentMethod>();
    }
}