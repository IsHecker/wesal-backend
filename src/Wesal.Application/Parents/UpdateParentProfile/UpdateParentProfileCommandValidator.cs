using FluentValidation;

namespace Wesal.Application.Parents.UpdateParentProfile;

internal sealed class UpdateParentProfileCommandValidator
    : AbstractValidator<UpdateParentProfileCommand>
{
    public UpdateParentProfileCommandValidator()
    {
        RuleFor(x => x.ParentId)
            .NotEmpty()
            .WithMessage("Parent ID is required.");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^\+?[\d\s\-\(\)]{7,15}$")
            .WithMessage("Phone number format is invalid.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required.")
            .MaximumLength(255)
            .WithMessage("Address must not exceed 255 characters.");
    }
}