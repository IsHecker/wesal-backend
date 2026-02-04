using FluentValidation;

namespace Wesal.Application.VisitationSchedules.ListVisitationSchedulesByFamily;

internal sealed class ListVisitationSchedulesByFamilyQueryValidator
    : AbstractValidator<ListVisitationSchedulesByFamilyQuery>
{
    public ListVisitationSchedulesByFamilyQueryValidator()
    {
        RuleFor(x => x.FamilyId).NotEmpty();
    }
}