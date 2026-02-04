using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.Alimonies;

namespace Wesal.Application.Alimonies.CreateAlimony;

public sealed class CreateAlimonyCommandValidator : AbstractValidator<CreateAlimonyCommand>
{
    public CreateAlimonyCommandValidator()
    {
        RuleFor(x => x.CourtCaseId).NotEmpty();
        RuleFor(x => x.PayerId).NotEmpty();
        RuleFor(x => x.RecipientId).NotEmpty();
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

        RuleFor(x => x)
            .Must(HaveDifferentPayerAndRecipient)
            .WithMessage("Payer and recipient must be different");
    }

    private bool HaveValidDateRange(CreateAlimonyCommand command) =>
        command.StartDate < command.EndDate;

    private bool HaveDifferentPayerAndRecipient(CreateAlimonyCommand command) =>
        command.PayerId != command.RecipientId;
}