using FluentValidation;

namespace Wesal.Application.CourtCases.EscalateToCourtCase;

internal sealed class EscalateToCourtCaseCommandValidator : AbstractValidator<EscalateToCourtCaseCommand>
{
    public EscalateToCourtCaseCommandValidator()
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family ID is required");

        RuleFor(x => x.CaseNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.DecisionSummary)
            .NotEmpty()
            .WithMessage("Decision summary is required")
            .MaximumLength(2000)
            .WithMessage("Decision summary cannot exceed 2000 characters")
            .MinimumLength(10)
            .WithMessage("Decision summary must be at least 10 characters");
    }
}