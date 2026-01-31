using Wesal.Application.Messaging;

namespace Wesal.Application.Notifications.MarkNotificationAsRead;

public record struct MarkNotificationAsReadCommand(
    Guid NotificationId,
    Guid UserId) : ICommand;