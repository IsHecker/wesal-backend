using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.VisitCenterStaffs;

public static class VisitCenterStaffErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("VisitCenterStaff.NotFound", $"Center staff with ID '{id}' was not found");
}