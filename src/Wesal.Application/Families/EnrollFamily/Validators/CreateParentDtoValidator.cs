using FluentValidation;
using Wesal.Application.Families.EnrollFamily.Dtos;
using Wesal.Domain.Common;

namespace Wesal.Application.Families.EnrollFamily.Validators;

internal sealed class CreateParentDtoValidator : AbstractValidator<CreateParentDto>
{
    public CreateParentDtoValidator()
    {
        RuleFor(x => x.NationalId)
            .NotEmpty()
            .WithMessage($"National ID is required")
            .Length(14)
            .WithMessage($"National ID must equal 14 characters")
            .Matches(@"^[0-9]+$")
            .WithMessage($"National ID must contain only digits");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage($"full name is required")
            .MaximumLength(200)
            .WithMessage($"full name cannot exceed 200 characters")
            .MinimumLength(2)
            .WithMessage($"full name must be at least 2 characters");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage($"birth date is required")
            .LessThan(EgyptTime.Today)
            .WithMessage($"birth date must be in the past")
            .Must(BeAtLeast18YearsOld)
            .WithMessage($"must be at least 18 years old");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage($"gender is required")
            .Must(BeValidGender)
            .WithMessage($"gender must be 'Male' or 'Female'");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage($"phone cannot exceed 20 characters")
            .Matches(@"^\+?[0-9\s\-()]+$")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone))
            .WithMessage($"phone number format is invalid");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage($"email address is invalid")
            .MaximumLength(255)
            .When(x => !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage($"email cannot exceed 255 characters");

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Address))
            .WithMessage($"address cannot exceed 500 characters");

        RuleFor(x => x.Job)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Job))
            .WithMessage($"job cannot exceed 100 characters");
    }

    private static bool BeAtLeast18YearsOld(DateOnly birthDate)
    {
        var age = AgeCalculator.CalculateAge(birthDate);

        return age >= 18;
    }

    private static bool BeValidGender(string gender)
    {
        return gender is "Male" or "Female";
    }
}