using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.SchoolReports;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Results;

namespace Wesal.Application.SchoolReports.UploadSchoolReport;

internal sealed class UploadSchoolReportCommandHandler(
    ISchoolRepository schoolRepository,
    IChildRepository childRepository,
    IRepository<SchoolReport> schoolReportRepository)
    : ICommandHandler<UploadSchoolReportCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        UploadSchoolReportCommand request,
        CancellationToken cancellationToken)
    {
        var school = await schoolRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (school is null)
            return SchoolErrors.NotFoundByUserId(request.UserId);

        var validationResult = await ValidateReport(request, school.Id, cancellationToken);
        if (validationResult.IsFailure)
            return validationResult.Error;

        var report = SchoolReport.Create(
            school.Id,
            request.ChildId,
            request.ReportType.ToEnum<SchoolReportType>());

        await schoolReportRepository.AddAsync(report, cancellationToken);

        return report.Id;
    }

    private async Task<Result> ValidateReport(
        UploadSchoolReportCommand request,
        Guid schoolId,
        CancellationToken cancellationToken)
    {
        var child = await childRepository.GetByIdAsync(request.ChildId, cancellationToken);
        if (child is null)
            return ChildErrors.NotFound(request.ChildId);

        if (child.SchoolId != schoolId)
            return SchoolReportErrors.ChildNotInSchool();

        return Result.Success;
    }
}