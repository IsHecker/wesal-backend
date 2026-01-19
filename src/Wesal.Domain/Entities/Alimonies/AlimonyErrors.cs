using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Alimonies;

public static class AlimonyErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("AlimonyObligation.NotFound", $"Alimony obligation with ID '{id}' was not found");

    public static Error AlreadyExists(Guid courtCaseId) =>
        Error.Conflict("Alimony.AlreadyExists", $"Alimony already exists for court case '{courtCaseId}'");

    public static Error PayerNotInFamily() =>
        Error.Validation("Alimony.PayerNotInFamily", "Payer is not part of the specified family");

    public static Error RecipientNotInFamily() =>
        Error.Validation("Alimony.RecipientNotInFamily", "Recipient is not part of the specified family");

    public static Error PayerAndRecipientSame() =>
        Error.Validation("Alimony.PayerAndRecipientSame", "Payer and recipient cannot be the same person");
}