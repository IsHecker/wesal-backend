namespace Wesal.Contracts.SchoolReports;

public record struct SchoolReportResponse(
    Guid Id,
    Guid ChildId,
    Guid SchoolId,
    Guid DocumentId,
    string ReportType,
    DateTime UploadedAt);