using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Parents;

public static class ParentErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Parent.NotFound", $"Parent with ID '{id}' was not found");

    public static Error NotFoundByNationalId(string nationalId) =>
        Error.NotFound(
            "Parent.NotFoundByNationalId",
            $"Parent with National ID '{nationalId}' was not found");
}