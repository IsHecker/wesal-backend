using FluentValidation;

namespace Wesal.Application.FamilyCourts.CreateFamilyCourt;

public sealed class CreateFamilyCourtCommandValidator : AbstractValidator<CreateFamilyCourtCommand>
{
    public CreateFamilyCourtCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Court name is required")
            .MaximumLength(200)
            .WithMessage("Court name must not exceed 200 characters");

        RuleFor(x => x.Governorate)
            .NotEmpty()
            .WithMessage("Governorate is required")
            .MaximumLength(100)
            .WithMessage("Governorate must not exceed 100 characters");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(500)
            .WithMessage("Address must not exceed 500 characters");

        RuleFor(x => x.ContactInfo)
            .NotEmpty()
            .WithMessage("Contact information is required")
            .MaximumLength(200)
            .WithMessage("Contact information must not exceed 200 characters");
    }
}