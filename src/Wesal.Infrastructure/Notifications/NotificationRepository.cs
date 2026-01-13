using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.Notifications;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Notifications;

internal sealed class NotificationRepository(WesalDbContext context)
    : Repository<Notification>(context), INotificationRepository
{
    public Task<IEnumerable<Notification>> GetByRecipientIdAsync(
        Guid recipientId,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<Notification>>(context.Notifications
            .Where(n => n.RecipientId == recipientId));
    }
}