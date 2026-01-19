using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Notifications;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Notifications.ListNotifications;

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
            ? notifications.Where(n => n.Status == NotificationStatus.Unread)
            : notifications;

        _ = notifications.TryGetNonEnumeratedCount(out var totalCount);
        _ = filteredNotifications.TryGetNonEnumeratedCount(out var unreadCount);

        var pagedNotifications = await filteredNotifications
            .OrderByDescending(n => n.SentAt)
            .Paginate(request.Pagination)
            .Select(n => new NotificationResponse(
                n.Id,
                n.Content,
                n.Type.ToString(),
                n.Status.ToString(),
                n.SentAt,
                n.ReadAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);

        return new ListNotificationsResponse(unreadCount, pagedNotifications);
    }
}