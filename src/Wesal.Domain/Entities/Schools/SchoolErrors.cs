using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Schools;

public static class SchoolErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("School.NotFound", $"School with ID '{id}' was not found");

    public static Error SchoolAlreadyExists(string name, string governorate) =>
        Error.Conflict("School.AlreadyExists", $"School '{name}' in '{governorate}' already exists");
}