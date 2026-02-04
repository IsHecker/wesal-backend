using Wesal.Application.Messaging;

namespace Wesal.Application.Notifications.GetUnreadCount;

public record struct GetUnreadCountQuery(Guid ParentId) : IQuery<int>;