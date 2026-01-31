using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Results;

namespace Wesal.Application.Notifications.GetUnreadCount;

internal sealed class GetUnreadCountQueryHandler(
    IWesalDbContext dbContext)
    : IQueryHandler<GetUnreadCountQuery, int>
{
    public async Task<Result<int>> Handle(
        GetUnreadCountQuery request,
        CancellationToken cancellationToken)
    {
        var count = await dbContext.Notifications
            .Where(n => n.RecipientId == request.UserId
                && n.Status == NotificationStatus.Sent)
            .CountAsync(cancellationToken);

        return count;
    }
}