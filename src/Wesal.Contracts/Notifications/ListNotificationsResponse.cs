using Wesal.Contracts.Common;

namespace Wesal.Contracts.Notifications;

public record struct ListNotificationsResponse(
    int UnreadCount,
    PagedResponse<NotificationResponse> Notifications);