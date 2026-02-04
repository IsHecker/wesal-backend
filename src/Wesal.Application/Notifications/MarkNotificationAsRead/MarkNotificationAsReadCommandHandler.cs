using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Results;

namespace Wesal.Application.Notifications.MarkNotificationAsRead;

internal sealed class MarkNotificationAsReadCommandHandler(
    IRepository<Notification> notificationRepository)
    : ICommandHandler<MarkNotificationAsReadCommand>
{
    public async Task<Result> Handle(
        MarkNotificationAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await notificationRepository.GetByIdAsync(
            request.NotificationId,
            cancellationToken);

        if (notification is null)
            return NotificationErrors.NotFound(request.NotificationId);

        if (notification.RecipientId != request.ParentId)
            return NotificationErrors.NotificationMismatch;

        notification.MarkAsRead();
        notificationRepository.Update(notification);

        return Result.Success;
    }
}