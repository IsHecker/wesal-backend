using FluentValidation;

namespace Wesal.Application.SchoolReports.ListSchoolReportsByChild;

public sealed class ListSchoolReportsByChildQueryValidator : AbstractValidator<ListSchoolReportsByChildQuery>
{
    public ListSchoolReportsByChildQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ChildId).NotEmpty();
    }
}