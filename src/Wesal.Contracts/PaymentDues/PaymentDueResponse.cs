namespace Wesal.Contracts.PaymentDues;

public record struct PaymentDueResponse(
    Guid Id,
    Guid AlimonyId,
    long Amount,
    DateTime DueDate,
    string Status,
    Guid? PaymentId = null,
    DateTime? PaidAt = null);