using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.SchoolReports;

namespace Wesal.Application.SchoolReports.ListSchoolReportsByChild;

public record struct ListSchoolReportsByChildQuery(
    Guid UserId,
    Guid ChildId,
    Pagination Pagination) : IQuery<PagedResponse<SchoolReportResponse>>;