using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.Alimonies;

namespace Wesal.Application.Alimonies.CreateAlimony;

public sealed class CreateAlimonyCommandValidator : AbstractValidator<CreateAlimonyCommand>
{
    public CreateAlimonyCommandValidator()
    {
        RuleFor(x => x.CourtCaseId).NotEmpty();
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero");

        RuleFor(x => x.Frequency)
            .NotEmpty()
            .MustBeEnumValue<CreateAlimonyCommand, AlimonyFrequency>();

        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .WithMessage("Start date must be before end date");
    }

    private bool HaveValidDateRange(CreateAlimonyCommand command) =>
        command.StartDate < command.EndDate;
}