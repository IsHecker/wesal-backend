using Wesal.Domain.DomainEvents;
using MediatR;

namespace Wesal.Application.Messaging;

public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;