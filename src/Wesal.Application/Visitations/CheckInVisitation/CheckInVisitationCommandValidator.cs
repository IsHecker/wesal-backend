using FluentValidation;

namespace Wesal.Application.Visitations.CheckInVisitation;

internal sealed class CheckInVisitationCommandValidator : AbstractValidator<CheckInVisitationCommand>
{
    public CheckInVisitationCommandValidator()
    {
        RuleFor(x => x.VisitationId)
            .NotEmpty()
            .WithMessage("Visitation ID is required");

        RuleFor(x => x.CenterStaffId)
            .NotEmpty()
            .WithMessage("Staff ID is required");
    }
}