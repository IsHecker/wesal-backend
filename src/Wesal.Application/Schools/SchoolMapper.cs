using Wesal.Contracts.Schools;
using Wesal.Domain.Entities.Schools;

namespace Wesal.Application.Schools;

internal static class SchoolMapper
{
    public static SchoolResponse ToResponse(this School school)
        => new(
            school.Id,
            school.Username,
            school.Name,
            school.Address,
            school.Governorate,
            school.Email,
            school.ContactNumber);
}