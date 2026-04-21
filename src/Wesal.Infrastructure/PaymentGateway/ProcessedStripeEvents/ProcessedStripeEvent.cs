using Wesal.Domain.Common;

namespace Wesal.Infrastructure.PaymentGateway.ProcessedStripeEvents;

public sealed class ProcessedStripeEvent(string eventId)
{
    public string EventId { get; init; } = eventId;
    public DateTime ProcessedAt { get; init; } = EgyptTime.UtcNow;
}