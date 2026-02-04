using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Children;

public static class ChildErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "Child.NotFound",
            $"Child with ID '{id}' was not found");

    public static readonly Error NotBelongsToFamily =
        Error.Validation(
            "Child.NotBelongsToFamily",
            "The specified child does not belong to the specified family.");

    public static readonly Error HasSchoolOrCustodyLinks =
        Error.Validation(
            "Child.HasSchoolOrCustodyLinks",
            "Cannot remove a child that has active school links or is referenced in a custody decision.");
}