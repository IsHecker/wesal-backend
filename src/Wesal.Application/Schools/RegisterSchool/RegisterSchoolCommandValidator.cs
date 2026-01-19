using FluentValidation;

namespace Wesal.Application.Schools.RegisterSchool;

public sealed class RegisterSchoolCommandValidator : AbstractValidator<RegisterSchoolCommand>
{
    public RegisterSchoolCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("School name is required")
            .MaximumLength(200)
            .WithMessage("School name cannot exceed 200 characters")
            .MinimumLength(3)
            .WithMessage("School name must be at least 3 characters");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.ContactNumber)
            .MaximumLength(20)
            .When(x => !string.IsNullOrWhiteSpace(x.ContactNumber))
            .WithMessage("Contact number cannot exceed 20 characters")
            .Matches(@"^\+?[0-9\s\-()]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.ContactNumber))
            .WithMessage("Invalid contact number format");
    }
}