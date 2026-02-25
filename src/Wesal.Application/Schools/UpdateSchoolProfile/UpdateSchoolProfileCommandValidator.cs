using FluentValidation;

namespace Wesal.Application.Schools.UpdateSchoolProfile;

public sealed class UpdateSchoolProfileCommandValidator : AbstractValidator<UpdateSchoolProfileCommand>
{
    public UpdateSchoolProfileCommandValidator()
    {
        RuleFor(x => x.SchoolId).NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.ContactNumber)
            .NotEmpty()
            .MaximumLength(20);
    }
}