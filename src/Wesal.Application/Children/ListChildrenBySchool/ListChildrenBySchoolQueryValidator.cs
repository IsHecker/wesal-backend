using FluentValidation;

namespace Wesal.Application.Children.ListChildrenBySchool;

internal sealed class ListChildrenBySchoolQueryValidator : AbstractValidator<ListChildrenBySchoolQuery>
{
    public ListChildrenBySchoolQueryValidator()
    {
        RuleFor(x => x.SchoolId).NotEmpty();
    }
}