using FluentValidation;

namespace Wesal.Application.Families.EnrollFamily.Validators;

public sealed class EnrollFamilyCommandValidator : AbstractValidator<EnrollFamilyCommand>
{
    public EnrollFamilyCommandValidator()
    {
        RuleFor(x => x.Father)
            .NotNull()
            .WithMessage("Father information is required")
            .SetValidator(new CreateParentDtoValidator());

        RuleFor(x => x.Mother)
            .NotNull()
            .WithMessage("Mother information is required")
            .SetValidator(new CreateParentDtoValidator());

        RuleForEach(x => x.Children)
            .SetValidator(new CreateChildDtoValidator())
            .When(x => x.Children is not null);

        RuleFor(x => x)
            .Must(HaveDifferentNationalIds)
            .WithMessage("Father and Mother must have different National IDs");
    }

    private bool HaveDifferentNationalIds(EnrollFamilyCommand command)
    {
        return command.Father.NationalId != command.Mother.NationalId;
    }
}