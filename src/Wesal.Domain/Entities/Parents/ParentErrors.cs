using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Parents;

public static class ParentErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Parent.NotFound", $"Parent with ID '{id}' was not found");
}