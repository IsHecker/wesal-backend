using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.CourtStaffs;

public static class CourtStaffErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "CourtStaff.NotFound",
            $"Court staff with ID '{id}' was not found");
}