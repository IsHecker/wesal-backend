using FluentValidation;

namespace Wesal.Application.CourtStaffs.CreateCourtStaff;

public sealed class CreateCourtStaffCommandValidator : AbstractValidator<CreateCourtStaffCommand>
{
    public CreateCourtStaffCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required")
            .MaximumLength(200)
            .WithMessage("Full name must not exceed 200 characters");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("Phone number must not exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.Phone));
    }
}