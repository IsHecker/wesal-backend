using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Notifications;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Results;

namespace Wesal.Application.Notifications.ListNotifications;

internal sealed class ListNotificationsByUserQueryHandler(INotificationRepository notificationRepository)
    : IQueryHandler<ListNotificationsByUserQuery, ListNotificationsResponse>
{
    public async Task<Result<ListNotificationsResponse>> Handle(
        ListNotificationsByUserQuery request,
        CancellationToken cancellationToken)
    {
        var allNotifications = await notificationRepository.GetByRecipientIdAsync(
            request.RecipientId,
            cancellationToken);

        var filteredNotifications = request.UnreadOnly
            ? allNotifications.Where(n => n.Status == NotificationStatus.Unread)
            : allNotifications;

        _ = allNotifications.TryGetNonEnumeratedCount(out var totalCount);
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