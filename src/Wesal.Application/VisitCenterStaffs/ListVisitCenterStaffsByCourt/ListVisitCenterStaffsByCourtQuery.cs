using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.VisitCenterStaffs;

namespace Wesal.Application.VisitCenterStaffs.ListVisitCenterStaffsByCourt;

public record struct ListVisitCenterStaffsByCourtQuery(
    Guid CourtId,
    Pagination Pagination) : IQuery<PagedResponse<VisitCenterStaffResponse>>;