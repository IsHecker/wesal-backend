using FluentValidation;
using Wesal.Application.Families.EnrollFamily.Dtos;

namespace Wesal.Application.Families.EnrollFamily.Validators;

public sealed class CreateChildDtoValidator : AbstractValidator<CreateChildDto>
{
    public CreateChildDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Child full name is required")
            .MaximumLength(200)
            .WithMessage("Child full name cannot exceed 200 characters")
            .MinimumLength(2)
            .WithMessage("Child full name must be at least 2 characters");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("Child birth date is required")
            .LessThan(DateTime.UtcNow)
            .WithMessage("Child birth date must be in the past");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage("Child gender is required")
            .Must(BeValidGender)
            .WithMessage("Child gender must be 'Male' or 'Female'");

        RuleFor(x => x.SchoolId)
            .NotEqual(Guid.Empty)
            .When(x => x.SchoolId.HasValue)
            .WithMessage("School ID cannot be empty GUID");
    }

    private bool BeValidGender(string gender)
    {
        return gender is "Male" or "Female";
    }
}