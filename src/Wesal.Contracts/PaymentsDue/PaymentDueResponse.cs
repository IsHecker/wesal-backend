namespace Wesal.Contracts.PaymentsDue;

public record struct PaymentDueResponse(
    Guid Id,
    Guid AlimonyId,
    long Amount,
    DateOnly DueDate,
    string Status,
    Guid? PaymentId = null,
    DateTime? PaidAt = null);