namespace Wesal.Infrastructure.PaymentGateway.StripeEvents;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
internal class StripeEventAttribute(string eventType) : Attribute
{
    public string EventType { get; } = eventType;
}