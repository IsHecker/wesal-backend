using FluentValidation;

namespace Wesal.Application.CourtCases.CloseCourtCase;

internal sealed class CloseCourtCaseCommandValidator : AbstractValidator<CloseCourtCaseCommand>
{
    public CloseCourtCaseCommandValidator()
    {
        RuleFor(x => x.CourtCaseId)
            .NotEmpty()
            .WithMessage("Court case ID is required.");

        RuleFor(x => x.CourtId)
            .NotEmpty()
            .WithMessage("Court ID is required.");

        RuleFor(x => x.ClosureNotes)
            .NotEmpty()
            .WithMessage("Closure notes are required when closing a case.")
            .MaximumLength(1000)
            .WithMessage("Closure notes must not exceed 1000 characters.");
    }
}