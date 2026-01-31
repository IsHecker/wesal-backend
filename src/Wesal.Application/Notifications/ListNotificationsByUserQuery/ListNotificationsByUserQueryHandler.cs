using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Notifications;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Notifications.ListNotificationsByUserQuery;

internal sealed class ListNotificationsByUserQueryHandler(
    INotificationRepository notificationRepository,
    IRepository<User> userRepository)
    : IQueryHandler<ListNotificationsByUserQuery, ListNotificationsResponse>
{
    public async Task<Result<ListNotificationsResponse>> Handle(
        ListNotificationsByUserQuery request,
        CancellationToken cancellationToken)
    {
        var isUserExist = await userRepository.ExistsAsync(request.UserId, cancellationToken);
        if (!isUserExist)
            return UserErrors.NotFound(request.UserId);

        var notifications = await notificationRepository.GetByRecipientIdAsync(
            request.UserId,
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