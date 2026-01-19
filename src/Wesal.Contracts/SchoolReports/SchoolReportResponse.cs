namespace Wesal.Contracts.SchoolReports;

public record struct SchoolReportResponse(
    Guid Id,
    Guid ChildId,
    string ReportType,
    DateTime UploadedAt);