using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.CourtCases;

namespace Wesal.Application.CourtCases.ListCourtCasesByStaff;

public record struct ListCourtCasesByStaffQuery(Guid StaffId, string? CaseNumber, Pagination Pagination)
    : IQuery<PagedResponse<CourtCaseResponse>>;