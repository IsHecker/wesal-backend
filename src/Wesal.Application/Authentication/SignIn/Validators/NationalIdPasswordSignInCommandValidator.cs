using FluentValidation;
using Wesal.Application.Authentication.Credentials;
using Wesal.Domain.Entities.Users;

namespace Wesal.Application.Authentication.SignIn.Validators;

public sealed class NationalIdPasswordSignInCommandValidator
    : AbstractValidator<SignInCommand<NationalIdPasswordCredentials>>
{
    public NationalIdPasswordSignInCommandValidator()
    {
        RuleFor(x => x.Credentials.NationalId)
            .NotEmpty()
            .WithMessage("National ID is required")
            .Length(14)
            .WithMessage("National ID must be 14 digits")
            .Matches(@"^\d{14}$")
            .WithMessage("National ID must contain only digits");

        RuleFor(x => x.Credentials.Password)
            .NotEmpty()
            .WithMessage("Password is required");

        RuleFor(x => x.UserRole)
            .NotEmpty()
            .WithMessage("User type is required")
            .Must(userRole => userRole == UserRole.Parent)
            .WithMessage("National ID credentials only valid for Parent user type");
    }
}