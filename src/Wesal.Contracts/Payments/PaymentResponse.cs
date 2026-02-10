namespace Wesal.Contracts.Payments;

public record struct PaymentResponse(
    Guid Id,
    string Method,
    string? ReceiptUrl,
    DateTime? PaidAt);