using FluentValidation;
using Wesal.Application.Authentication.Credentials;
using Wesal.Domain.Entities.Users;

namespace Wesal.Application.Authentication.SignIn.Validators;

public sealed class EmailPasswordSignInValidator
    : AbstractValidator<SignInCommand<EmailPasswordCredentials>>
{
    public EmailPasswordSignInValidator()
    {
        RuleFor(x => x.Credentials.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format");

        RuleFor(x => x.Credentials.Password)
            .NotEmpty()
            .WithMessage("Password is required");

        RuleFor(x => x.UserRole)
            .NotEmpty()
            .WithMessage("User type is required")
            .Must(BeValidUserRole)
            .WithMessage("Invalid user type");
    }

    private bool BeValidUserRole(string userRole)
    {
        return userRole is UserRole.SystemAdmin
            or UserRole.FamilyCourt
            or UserRole.CourtStaff
            or UserRole.School
            or UserRole.VisitCenterStaff;
    }
}