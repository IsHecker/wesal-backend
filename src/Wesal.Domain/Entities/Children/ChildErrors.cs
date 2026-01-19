using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Children;

public static class ChildErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "Child.NotFound",
            $"Child with ID '{id}' was not found");
}