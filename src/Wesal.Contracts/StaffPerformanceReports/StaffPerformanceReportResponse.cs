namespace Wesal.Contracts.StaffPerformanceReports;

public record struct StaffPerformanceReportResponse(
    Guid StaffId,
    string FullName,
    string Role,
    
    // Settlement Specialist Metrics
    int? TotalFamiliesEnrolled = null,
    int? SuccessfulSettlements = null,
    int? EscalatedFamilies = null,
    
    // Case Clerk Metrics
    int? TotalCasesAssigned = null,
    int? CasesClosed = null,
    
    // Compliance Monitor Metrics
    int? TotalComplaintsAssigned = null,
    int? ComplaintsResolved = null,
    int? TotalAlertsAssigned = null,
    int? AlertsResolved = null,
    int? RefiledComplaints = null,
    double? AverageComplaintResolutionTimeDays = null,
    double? AverageAlertResolutionTimeDays = null,
    
    // Shared Metrics
    int CurrentlyOpenItems = 0,
    double? AverageResolutionTimeDays = null);
