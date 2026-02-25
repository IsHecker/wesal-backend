using Wesal.Contracts.VisitCenterStaffs;
using Wesal.Domain.Entities.VisitCenterStaffs;

namespace Wesal.Application.VisitCenterStaffs;

internal static class VisitCenterStaffMapper
{
    public static VisitCenterStaffResponse ToResponse(this VisitCenterStaff staff)
        => new(
            staff.Id,
            staff.CourtId,
            staff.LocationId,
            staff.FullName,
            staff.Email,
            staff.Phone);
}