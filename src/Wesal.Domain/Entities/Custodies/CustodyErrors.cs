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

    public static readonly Error NewCustodianNotInFamily = Error.Validation(
        code: "Custody.NewCustodianNotInFamily",
        description: "The specified custodian is not a parent in this family.");

    public static readonly Error HasDependentRecords = Error.Validation(
        code: "Custody.HasDependentRecords",
        description: "Cannot delete a custody record that has dependent visitations or alimony obligations. It is the legal backbone of the case.");

    public static readonly Error CannotModifyClosedCase = Error.Validation(
        code: "Custody.CannotModifyClosedCase",
        description: "Cannot modify a custody record for a closed court case.");
}