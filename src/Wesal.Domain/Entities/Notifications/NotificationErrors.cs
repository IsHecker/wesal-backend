using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Notifications;

public static class NotificationErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "Notifications.NotFound",
            $"Notification with ID '{id}' was not found");

    public static Error NotificationMismatch =>
        Error.Unauthorized("Notifications.NotificationMismatch", "You are not authorized to manage this notification");

    public static readonly Error SendFailed =
        Error.Failure(
            "Notifications.SendFailed",
            "Failed to send push notification via FCM");

    public static readonly Error InvalidDeviceToken =
        Error.Validation(
            "Notifications.InvalidDeviceToken",
            "Device token is invalid or malformed");

    public static Error DeviceNotFound(Guid userId) =>
        Error.NotFound(
            "Notifications.DeviceNotFound",
            $"No active devices found for user '{userId}'");
}
