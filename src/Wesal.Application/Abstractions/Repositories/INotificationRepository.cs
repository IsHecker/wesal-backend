using Wesal.Application.Data;
using Wesal.Domain.Entities.Notifications;

namespace Wesal.Application.Abstractions.Repositories;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetByRecipientIdAsync(Guid recipientId, CancellationToken cancellationToken = default);
}