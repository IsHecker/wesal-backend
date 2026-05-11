namespace Wesal.Contracts.StaffPerformanceReports;

public record struct StaffPerformanceSummaryResponse(
    Guid StaffId,
    string FullName,
    string Role,
    int TotalAssigned,
    int TotalResolved,
    int CurrentlyOpenItems,
    double? AverageResolutionTimeDays);