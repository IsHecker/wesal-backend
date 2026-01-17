using FluentValidation;

namespace Wesal.Application.Schools.ListSchools;

public sealed class ListSchoolsQueryValidator : AbstractValidator<ListSchoolsQuery>
{
    public ListSchoolsQueryValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Name))
            .WithMessage("Name filter cannot exceed 100 characters");
    }
}