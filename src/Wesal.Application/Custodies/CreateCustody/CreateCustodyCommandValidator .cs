using FluentValidation;

namespace Wesal.Application.Custodies.CreateCustody;

internal sealed class CreateCustodyCommandValidator : AbstractValidator<CreateCustodyCommand>
{
    public CreateCustodyCommandValidator()
    {
        RuleFor(x => x.CourtId).NotEmpty();
        RuleFor(x => x.CourtCaseId).NotEmpty();
        RuleFor(x => x.CustodianId).NotEmpty();

        RuleFor(x => x.StartAt)
            .NotEmpty()
            .WithMessage("Start date is required");

        RuleFor(x => x)
            .Must(HaveValidDateRange)
            .When(x => x.EndAt.HasValue)
            .WithMessage("Start date must be before end date");
    }

    private bool HaveValidDateRange(CreateCustodyCommand command) =>
        !command.EndAt.HasValue || command.StartAt < command.EndAt.Value;
}