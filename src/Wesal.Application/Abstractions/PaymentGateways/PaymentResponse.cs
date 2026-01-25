namespace Wesal.Application.Abstractions.PaymentGateways;

public record PaymentResponse
{
    public bool IsSuccess { get; init; }
    public string? TransactionId { get; init; }
    public string? ErrorMessage { get; init; }
    public string Status { get; init; } = null!;
    public string? GatewayResponse { get; init; }
    public DateTime ProcessedAt { get; init; } = DateTime.UtcNow;
    public Dictionary<string, string> AdditionalData { get; init; } = new();

    public static PaymentResponse Success(
        string transactionId,
        string? gatewayResponse = null,
        Dictionary<string, string>? additionalData = null)
        => new()
        {
            IsSuccess = true,
            TransactionId = transactionId,
            Status = "Completed",
            GatewayResponse = gatewayResponse,
            AdditionalData = additionalData ?? new()
        };

    public static PaymentResponse Failure(
        string error,
        string? gatewayResponse = null)
        => new()
        {
            IsSuccess = false,
            ErrorMessage = error,
            Status = "Failed",
            GatewayResponse = gatewayResponse
        };

    public static PaymentResponse Pending(string? transactionId = null)
        => new()
        {
            IsSuccess = true,
            TransactionId = transactionId,
            Status = "Pending"
        };
}