namespace Wesal.Contracts.Payments;

public record struct PaymentResponse(
    Guid Id,
    long Amount,
    string Currency,
    string Status,
    string Method,
    string ReceiptUrl,
    DateTime? PaidAt);