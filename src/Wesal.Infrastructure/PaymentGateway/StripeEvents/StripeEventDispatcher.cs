using System.Collections.Frozen;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stripe;
using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Domain.Results;
using Wesal.Infrastructure.PaymentGateway.ProcessedStripeEvents;

namespace Wesal.Infrastructure.PaymentGateway.StripeEvents;

internal sealed class StripeEventDispatcher
    : IStripeEventDispatcher
{
    private static FrozenDictionary<string, Type> _handlersType = null!;
    private static StripeOptions _stripeOptions = null!;

    private readonly IServiceProvider _serviceProvider;
    private readonly ProcessedStripeEventRepository _eventRepository;

    public StripeEventDispatcher(
        StripeOptions stripeOptions,
        IServiceProvider serviceProvider,
        ProcessedStripeEventRepository eventRepository)
    {
        if (_handlersType is null)
            RegisterHandlersFromAssembly(AssemblyReference.Assembly);

        _stripeOptions = stripeOptions;
        _serviceProvider = serviceProvider;
        _eventRepository = eventRepository;
    }

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
                isConnectWebhook ? _stripeOptions.ConnectWebhookSecret
                    : _stripeOptions.WebhookSecret,
                throwOnApiVersionMismatch: false);
        }
        catch (StripeException)
        {
            return Error.Validation();
        }

        if (await _eventRepository.IsEventProcessedAsync(stripeEvent.Id))
            return Result.Success;

        if (!_handlersType.TryGetValue(stripeEvent.Type, out var handlerType))
            return Result.Success;

        var handler = (IStripeEventHandler)ActivatorUtilities.CreateInstance(_serviceProvider, handlerType);

        var result = await handler.HandleAsync(stripeEvent);

        if (result.IsSuccess)
        {
            await _eventRepository.MarkEventAsProcessedAsync(stripeEvent.Id);
        }

        return result;
    }

    private static void RegisterHandlersFromAssembly(Assembly assembly)
    {
        _handlersType = assembly.GetTypes()
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