using FluentValidation;
using Wesal.Application.Custodies.UpdateCustody;

internal sealed class UpdateCustodyCommandValidator : AbstractValidator<UpdateCustodyCommand>
{
    public UpdateCustodyCommandValidator()
    {
        RuleFor(x => x.CustodyId)
            .NotEmpty()
            .WithMessage("Custody ID is required.");

        RuleFor(x => x.NewCustodianId)
            .NotEmpty()
            .WithMessage("New custodian ID is required.");

        RuleFor(x => x.StartAt)
            .NotEmpty()
            .WithMessage("Start date is required");

        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .When(x => x.EndAt.HasValue)
            .WithMessage("Start date must be before end date");
    }

    private bool HaveValidDateRange(UpdateCustodyCommand command) =>
        !command.EndAt.HasValue || command.StartAt < command.EndAt.Value;
}