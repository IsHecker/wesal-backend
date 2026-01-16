namespace Wesal.Contracts.Payments;

public record struct PaymentResponse(
    Guid Id,
    long Amount,
    string Method,
    string ReceiptUrl,
    DateTime? PaidAt);