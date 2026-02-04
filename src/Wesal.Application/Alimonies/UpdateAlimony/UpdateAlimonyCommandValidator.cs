using FluentValidation;
using Wesal.Application.Alimonies.UpdateAlimony;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.Alimonies;

internal sealed class UpdateAlimonyCommandValidator : AbstractValidator<UpdateAlimonyCommand>
{
    public UpdateAlimonyCommandValidator()
    {
        RuleFor(x => x.AlimonyId)
            .NotEmpty()
            .WithMessage("Alimony ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Frequency).MustBeEnumValue<UpdateAlimonyCommand, AlimonyFrequency>();

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Due date is required.");
    }
}