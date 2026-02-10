using Stripe;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.PaymentGateway.StripeEvents;

internal interface IStripeEventHandler
{
    Task<Result> HandleAsync(Event stripeEvent);
}

internal abstract class StripeEventHandler<TEntity> : IStripeEventHandler
    where TEntity : class, IStripeEntity
{
    protected abstract Task<Result> HandleAsync(TEntity stripeEvent);

    async Task<Result> IStripeEventHandler.HandleAsync(Event stripeEvent)
    {
        if (stripeEvent.Data.Object is not TEntity entity)
            return Error.Validation();

        return await HandleAsync(entity);
    }
}