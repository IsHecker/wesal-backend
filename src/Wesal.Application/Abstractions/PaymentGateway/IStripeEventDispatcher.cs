using Wesal.Domain.Results;

namespace Wesal.Application.Abstractions.PaymentGateway;

public interface IStripeEventDispatcher
{
    Task<Result> DispatchAsync(string eventJson, string stripeSignature, bool isConnectWebhook = false);
}