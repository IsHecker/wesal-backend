using Wesal.Application.Messaging;

namespace Wesal.Application.Notifications.MarkNotificationAsRead;

public record struct MarkNotificationAsReadCommand(
    Guid ParentId,
    Guid NotificationId) : ICommand;