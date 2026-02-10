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
    public async Task<Result> SendNotificationsAsync(
        IReadOnlyList<Notification> notifications,
        Dictionary<string, string>? sharedData = null,
        CancellationToken cancellationToken = default)
    {
        if (notifications.Count == 0)
            return Result.Success;

        var saveResult = await SaveNotificationBatchAsync(notifications, cancellationToken);
        if (saveResult.IsFailure)
            return saveResult.Error;

        var result = await SendToUsersAsync(
            notifications,
            sharedData,
            CancellationToken.None);

        if (result.IsFailure)
            logger.LogWarning("Failed to send push notification");

        return Result.Success;
    }

    private async Task<Result> SaveNotificationBatchAsync(
        IReadOnlyList<Notification> notifications,
        CancellationToken cancellationToken)
    {
        try
        {
            await notificationRepository.AddRangeAsync(notifications, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to save batch of {Count} notifications",
                notifications.Count);

            foreach (var notification in notifications)
            {
                dbContext.Entry(notification).State = EntityState.Detached;
            }

            return Error.Failure();
        }
    }

    private async Task<Result> SendToUsersAsync(
        IReadOnlyList<Notification> notifications,
        Dictionary<string, string>? sharedData = null,
        CancellationToken cancellationToken = default)
    {
        var notification = notifications[0];

        var userIds = notifications.Select(n => n.RecipientId);
        var devices = dbContext.UserDevices.Where(device => userIds.Contains(device.UserId) && device.IsActive);

        if (!await devices.AnyAsync(cancellationToken))
        {
            logger.LogWarning("No active devices found");
            return Error.NotFound();
        }

        var deviceTokens = await devices.Select(device => device.DeviceToken).ToListAsync(cancellationToken);

        return await fcmService.SendToDevicesAsync(
            deviceTokens,
            notification.Title,
            notification.Content,
            sharedData,
            cancellationToken);
    }
}