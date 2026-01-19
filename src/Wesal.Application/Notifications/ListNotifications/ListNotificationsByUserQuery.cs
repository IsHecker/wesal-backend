using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Notifications;

namespace Wesal.Application.Notifications.ListNotifications;

public record struct ListNotificationsByUserQuery(
    Guid UserId,
    bool UnreadOnly,
    Pagination Pagination) : IQuery<ListNotificationsResponse>;