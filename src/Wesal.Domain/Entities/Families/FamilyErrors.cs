using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Families;

public static class FamilyErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Family.NotFound", $"Family with ID '{id}' was not found");

    public static Error NotFoundByParent(Guid parentId) =>
        Error.NotFound("Family.NotFound", $"Family with Parent ID '{parentId}' was not found");

    public static Error ParentNotInFamily =>
        Error.Unauthorized("Family.ParentNotInFamily", $"This parent is not in the current family.");

    public static Error ParentAlreadyExists(string nationalId) =>
        Error.Conflict(
            "Family.ParentAlreadyExists",
            $"Parent with National ID '{nationalId}' already exists");

    public static readonly Error HasActiveCaseOrEvidence =
        Error.Validation(
            "Family.HasActiveCaseOrEvidence",
            "Cannot delete a family that has an active court case or existing legal evidence (completed visitations or paid alimony).");
}