using FluentValidation;
using Wesal.Application.Children.AddChild;

internal sealed class AddChildCommandValidator : AbstractValidator<AddChildCommand>
{
    public AddChildCommandValidator()
    {
        RuleFor(x => x.FamilyId)
            .NotEmpty()
            .WithMessage("Family ID is required.");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Child's full name is required.")
            .MinimumLength(2)
            .WithMessage("Full name must be at least 2 characters.")
            .MaximumLength(100)
            .WithMessage("Full name must not exceed 100 characters.");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .WithMessage("Date of birth is required.")
            .Must(dob => dob <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth must not be in the future.");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage("Gender is required.")
            .Must(g => g == "Male" || g == "Female")
            .WithMessage("Gender must be 'Male' or 'Female'.");
    }
}