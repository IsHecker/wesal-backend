using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.CourtStaffs;

namespace Wesal.Application.CourtStaffs.ListCourtStaffsByCourt;

public record struct ListCourtStaffsByCourtQuery(
    Guid CourtId,
    string? Role,
    Pagination Pagination) : IQuery<PagedResponse<CourtStaffResponse>>;