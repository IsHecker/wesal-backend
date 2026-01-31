using Wesal.Application.Messaging;

namespace Wesal.Application.SchoolReports.UploadSchoolReport;

public record struct UploadSchoolReportCommand(
    Guid SchooldId,
    Guid ChildId,
    Guid DocumentId,
    string ReportType) : ICommand<Guid>;