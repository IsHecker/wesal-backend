using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.SchoolReports;
using Wesal.Domain.Results;

namespace Wesal.Application.SchoolReports.UploadSchoolReport;

internal sealed class UploadSchoolReportCommandHandler(
    IChildRepository childRepository,
    IRepository<SchoolReport> schoolReportRepository,
    INotificationService notificationService,
    IFamilyRepository familyRepository)
    : ICommandHandler<UploadSchoolReportCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        UploadSchoolReportCommand request,
        CancellationToken cancellationToken)
    {
        var child = await childRepository.GetByIdAsync(request.ChildId, cancellationToken);
        if (child is null)
            return ChildErrors.NotFound(request.ChildId);

        if (child.SchoolId != request.SchooldId)
            return SchoolReportErrors.ChildNotInSchool();

        var family = await familyRepository.GetByIdAsync(child.FamilyId, cancellationToken);

        var report = SchoolReport.Create(
            request.ChildId,
            request.SchooldId,
            request.DocumentId,
            request.ReportType.ToEnum<SchoolReportType>());

        await schoolReportRepository.AddAsync(report, cancellationToken);

        await SendNotificationAsync(child, family!, report, cancellationToken);

        return report.Id;
    }

    private async Task SendNotificationAsync(
        Child child,
        Family family,
        SchoolReport report,
        CancellationToken cancellationToken)
    {
        await notificationService.SendNotificationsAsync(
            [
                NotificationTemplate.SchoolReportUploaded(family!.FatherId, child.FullName),
                NotificationTemplate.SchoolReportUploaded(family.MotherId, child.FullName)
            ],
            new Dictionary<string, string>
            {
                ["schoolReportId"] = report.Id.ToString()
            },
            cancellationToken);
    }
}