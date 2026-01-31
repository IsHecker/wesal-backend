using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Results;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Notifications.Services;

internal sealed class NotificationService(
    IRepository<Notification> notificationRepository,
    FcmService fcmService,
    WesalDbContext dbContext,
    ILogger<NotificationService> logger) : INotificationService
{
    public async Task<Result> SendNotificationAsync(
        Notification notification,
        Dictionary<string, string>? data = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await notificationRepository.AddAsync(notification, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            var pushData = data ?? [];
            pushData["notificationId"] = notification.Id.ToString();
            pushData["type"] = notification.Type.ToString();

            var result = await SendToUserAsync(
                notification,
                pushData,
                CancellationToken.None);

            if (result.IsFailure)
            {
                logger.LogWarning(
                    "Failed to send push notification for notification {NotificationId}: {Error}",
                    notification.Id,
                    result.Error.Description);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Exception occurred while sending push notification for notification {NotificationId}",
                notification.Id);
        }

        return Result.Success;
    }

    private async Task<Result> SendToUserAsync(
        Notification notification,
        Dictionary<string, string>? data = null,
        CancellationToken cancellationToken = default)
    {
        var userId = notification.RecipientId;
        var devices = dbContext.UserDevices.Where(device => device.UserId == userId && device.IsActive);

        if (!await devices.AnyAsync(cancellationToken))
        {
            logger.LogWarning("No active devices found for user {UserId}", userId);
            return NotificationErrors.DeviceNotFound(userId);
        }

        var deviceTokens = await devices.Select(device => device.DeviceToken).ToListAsync(cancellationToken);

        return await fcmService.SendToDevicesAsync(deviceTokens, notification.Title, notification.Content, data, cancellationToken);
    }
}