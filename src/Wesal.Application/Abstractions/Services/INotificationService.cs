using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Results;

namespace Wesal.Application.Abstractions.Services;

public interface INotificationService
{
    Task<Result> SendNotificationAsync(
        Notification notification,
        Dictionary<string, string>? data = null,
        CancellationToken cancellationToken = default);
}