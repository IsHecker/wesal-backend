using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Results;

namespace Wesal.Application.Abstractions.Services;

public interface INotificationService
{
    Task<Result> SendNotificationsAsync(
        IReadOnlyList<Notification> notifications,
        Dictionary<string, string>? sharedData = null,
        CancellationToken cancellationToken = default);
}