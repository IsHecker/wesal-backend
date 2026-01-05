using MediatR;

namespace Wesal.Domain.DomainEvents;

public interface IDomainEvent : INotification
{
    Guid Id { get; }

    DateTime OccurredOnUtc { get; }
}