using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Custodies;

public static class CustodyErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Custody.NotFound", $"Custody with ID '{id}' was not found");

    public static Error NotFoundForFamily(Guid familyId) =>
        Error.NotFound(
            "Custody.NotFoundForFamily",
            $"No custody decision found for family with ID '{familyId}'");

    public static Error AlreadyExists(Guid courtCaseId) =>
        Error.Conflict("Custody.AlreadyExists", $"Custody already exists for court case '{courtCaseId}'");

    public static Error CustodianNotInFamily =>
        Error.Validation("Custody.CustodianNotInFamily", "Custodian is not part of the specified family");
}