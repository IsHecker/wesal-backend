using FluentValidation;
using Wesal.Application.Extensions;
using Wesal.Domain.Entities.SchoolReports;

namespace Wesal.Application.SchoolReports.UploadSchoolReport;

public sealed class UploadSchoolReportCommandValidator : AbstractValidator<UploadSchoolReportCommand>
{
    public UploadSchoolReportCommandValidator()
    {
        RuleFor(x => x.SchooldId).NotEmpty();
        RuleFor(x => x.ChildId).NotEmpty();

        RuleFor(x => x.ReportType)
            .NotEmpty()
            .MustBeEnumValue<UploadSchoolReportCommand, SchoolReportType>();
    }
}