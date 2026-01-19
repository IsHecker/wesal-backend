using Wesal.Application.Messaging;

namespace Wesal.Application.SchoolReports.UploadSchoolReport;

public record struct UploadSchoolReportCommand(
    Guid UserId,
    Guid ChildId,
    string ReportType) : ICommand<Guid>;