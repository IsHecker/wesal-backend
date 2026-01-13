using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.CourtCases;

namespace Wesal.Application.CourtCases.CreateCourtCase;

public sealed class CreateCourtCaseCommandValidator : AbstractValidator<CreateCourtCaseCommand>
{
    public CreateCourtCaseCommandValidator()
    {
        RuleFor(x => x.CourtId)
            .NotEmpty()
            .WithMessage("Court ID is required");

        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family ID is required");

        RuleFor(x => x.CaseNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.FiledAt)
            .NotEmpty()
            .WithMessage("Filed date is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Filed date cannot be in the future");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Case status is required")
            .MustBeEnumValue<CreateCourtCaseCommand, CourtCaseStatus>();

        RuleFor(x => x.DecisionSummary)
            .NotEmpty()
            .WithMessage("Decision summary is required")
            .MaximumLength(2000)
            .WithMessage("Decision summary cannot exceed 2000 characters")
            .MinimumLength(10)
            .WithMessage("Decision summary must be at least 10 characters");
    }
}