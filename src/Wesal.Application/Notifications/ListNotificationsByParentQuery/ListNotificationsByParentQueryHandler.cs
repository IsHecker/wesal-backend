using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Notifications;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Results;

namespace Wesal.Application.Notifications.ListNotificationsByParentQuery;

internal sealed class ListNotificationsByParentQueryHandler(
    INotificationRepository notificationRepository)
    : IQueryHandler<ListNotificationsByParentQuery, ListNotificationsResponse>
{
    public async Task<Result<ListNotificationsResponse>> Handle(
        ListNotificationsByParentQuery request,
        CancellationToken cancellationToken)
    {
        var notifications = await notificationRepository.GetByRecipientIdAsync(
            request.ParentId,
            cancellationToken);

        var filteredNotifications = request.UnreadOnly
            ? notifications.Where(notification => notification.Status == NotificationStatus.Unread)
            : notifications;

        var totalCount = notifications.Count();
        var unreadCount = filteredNotifications.Count();

        var pagedNotifications = await filteredNotifications
            .OrderByDescending(notification => notification.SentAt)
            .Paginate(request.Pagination)
            .Select(notification => new NotificationResponse(
                notification.Id,
                notification.Content,
                notification.Type.ToString(),
                notification.Status.ToString(),
                notification.SentAt,
                notification.ReadAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);

        return new ListNotificationsResponse(unreadCount, pagedNotifications);
    }
}