using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Notifications;

namespace Wesal.Application.Notifications.ListNotificationsByParentQuery;

public record struct ListNotificationsByParentQuery(
    Guid ParentId,
    bool UnreadOnly,
    Pagination Pagination) : IQuery<ListNotificationsResponse>;