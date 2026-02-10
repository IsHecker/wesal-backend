namespace Wesal.Infrastructure.PaymentGateway;

internal sealed class StripeOptions
{
    public const string SectionName = "Stripe";

    public string SecretKey { get; init; } = null!;
    public string WebhookSecret { get; init; } = null!;
    public string ConnectWebhookSecret { get; init; } = null!;
}