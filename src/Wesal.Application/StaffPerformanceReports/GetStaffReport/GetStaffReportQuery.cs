using Wesal.Application.Messaging;
using Wesal.Contracts.StaffPerformanceReports;

namespace Wesal.Application.StaffPerformanceReports.GetStaffReport;

public record struct GetStaffReportQuery(Guid StaffId) : IQuery<StaffPerformanceReportResponse>;