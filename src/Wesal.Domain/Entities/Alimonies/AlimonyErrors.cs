using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Alimonies;

public static class AlimonyErrors
{
    public static Error NotFound =>
        Error.NotFound("AlimonyObligation.NotFound", $"Alimony obligation is not found");

    public static Error AlreadyExists(Guid courtCaseId) =>
        Error.Conflict("Alimony.AlreadyExists", $"Alimony already exists for court case '{courtCaseId}'");

    public static Error CustodyNotFound =>
        Error.Validation("Alimony.CustodyNotFound", "No custody rule was found for this family.");

    public static readonly Error CannotModifyClosedCase = Error.Validation(
        code: "Alimony.CannotModifyClosedCase",
        description: "Cannot modify alimony belonging to a closed court case.");
}