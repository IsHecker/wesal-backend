using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Alimonies;

public static class AlimonyErrors
{
    public static Error NotFound =>
        Error.NotFound("AlimonyObligation.NotFound", $"Alimony obligation is not found");

    public static Error AlreadyExists(Guid courtCaseId) =>
        Error.Conflict("Alimony.AlreadyExists", $"Alimony already exists for court case '{courtCaseId}'");

    public static Error PayerNotInFamily =>
        Error.Validation("Alimony.PayerNotInFamily", "Payer is not part of the specified family");

    public static Error RecipientNotInFamily =>
        Error.Validation("Alimony.RecipientNotInFamily", "Recipient is not part of the specified family");

    public static Error PayerAndRecipientSame =>
        Error.Validation("Alimony.PayerAndRecipientSame", "Payer and recipient cannot be the same person");

    public static readonly Error CannotUpdatePaid = Error.Validation(
        code: "Alimony.CannotUpdatePaid",
        description: "Cannot update an alimony record that has already been paid.");

    public static readonly Error CannotDeletePaid = Error.Validation(
        code: "Alimony.CannotDeletePaid",
        description: "Cannot delete an alimony record that has been paid. It is legal evidence.");

    public static readonly Error CannotModifyClosedCase = Error.Validation(
        code: "Alimony.CannotModifyClosedCase",
        description: "Cannot modify alimony belonging to a closed court case.");
}