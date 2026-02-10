using FluentValidation;

namespace Wesal.Application.Visitations.SetCompanionForVisitation;

internal sealed class SetCompanionForVisitationCommandValidator : AbstractValidator<SetCompanionForVisitationCommand>
{
    public SetCompanionForVisitationCommandValidator()
    {
        RuleFor(x => x.CustodialParentId).NotEmpty();
        RuleFor(x => x.VisitationId).NotEmpty();

        RuleFor(x => x.CompanionNationalId)
            .NotEmpty()
            .WithMessage($"National ID is required")
            .Length(14)
            .WithMessage($"National ID must equal 14 characters")
            .Matches(@"^[0-9]+$")
            .WithMessage($"National ID must contain only digits");
    }
}