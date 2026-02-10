using FluentValidation;

namespace Wesal.Application.VisitationSchedules.GetVisitationScheduleByCourtCase;

internal sealed class GetVisitationScheduleByCourtCaseQueryValidator
    : AbstractValidator<GetVisitationScheduleByCourtCaseQuery>
{
    public GetVisitationScheduleByCourtCaseQueryValidator()
    {
        RuleFor(x => x.CourtCaseId).NotEmpty();
    }
}