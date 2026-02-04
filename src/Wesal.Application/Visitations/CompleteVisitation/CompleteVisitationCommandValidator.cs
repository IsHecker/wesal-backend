using FluentValidation;

namespace Wesal.Application.Visitations.CompleteVisitation;

internal sealed class CompleteVisitationCommandValidator : AbstractValidator<CompleteVisitationCommand>
{
    public CompleteVisitationCommandValidator()
    {
        RuleFor(x => x.VisitationId)
            .NotEmpty()
            .WithMessage("Visitation ID is required");

        RuleFor(x => x.CenterStaffId)
            .NotEmpty()
            .WithMessage("Center Staff ID is required");
    }
}