using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.StaffPerformanceReports;

namespace Wesal.Application.StaffPerformanceReports.ListStaffReports;

public record struct ListStaffReportsQuery(
    Guid CourtId,
    string? Role,
    Pagination Pagination) : IQuery<PagedResponse<StaffPerformanceSummaryResponse>>;