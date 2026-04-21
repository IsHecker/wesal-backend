using System.Collections.Frozen;
using System.Reflection;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Domain.Results;
using Wesal.Infrastructure.PaymentGateway.ProcessedStripeEvents;

namespace Wesal.Infrastructure.PaymentGateway.StripeEvents;

internal sealed class StripeEventDispatcher(
    StripeOptions stripeOptions,
    IServiceProvider serviceProvider,
    ProcessedStripeEventRepository eventRepository) : IStripeEventDispatcher
{
    private static readonly FrozenDictionary<string, Type> _handlersType = RegisterHandlersFromAssembly(AssemblyReference.Assembly);

    public async Task<Result> DispatchAsync(
        string eventJson,
        string stripeSignature,
        bool isConnectWebhook = false)
    {
        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                eventJson,
                stripeSignature,
                isConnectWebhook ? stripeOptions.ConnectWebhookSecret
                    : stripeOptions.WebhookSecret,
                throwOnApiVersionMismatch: false);
        }
        catch (StripeException)
        {
            return Error.Validation();
        }

        if (await eventRepository.IsEventProcessedAsync(stripeEvent.Id))
            return Result.Success;

        if (!_handlersType.TryGetValue(stripeEvent.Type, out var handlerType))
            return Result.Success;

        var handler = (IStripeEventHandler)ActivatorUtilities.CreateInstance(serviceProvider, handlerType);

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            Result result = await handler.HandleAsync(stripeEvent);

            if (result.IsSuccess)
            {
                await eventRepository.MarkEventAsProcessedAsync(stripeEvent.Id);
                scope.Complete();
            }

            return result;
        }
        catch (Exception)
        {
            // If another webhook is processing this concurrently, the unique key constraint will fail it here
            return Result.Success;
        }
    }

    private static FrozenDictionary<string, Type> RegisterHandlersFromAssembly(Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(type => type.IsClass
                        && !type.IsAbstract
                        && type.IsAssignableTo(typeof(IStripeEventHandler)))
            .Select(type => new
            {
                type.GetCustomAttribute<StripeEventAttribute>()!.EventType,
                HandlerType = type
            })
            .ToFrozenDictionary(x => x.EventType, x => x.HandlerType, StringComparer.Ordinal);
    }
}