using FirebaseAdmin.Messaging;
using Microsoft.Extensions.Logging;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.Notifications.Services;

internal sealed class FcmService(
    FirebaseMessaging messaging,
    ILogger<FcmService> logger)
{
    public async Task<Result> SendToDeviceAsync(
        string deviceToken,
        string title,
        string body,
        Dictionary<string, string>? data = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var message = new Message
            {
                Token = deviceToken,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data ?? [],
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Sound = "default",
                        ChannelId = "default"
                    }
                }
            };

            var response = await messaging.SendAsync(message, cancellationToken);

            logger.LogInformation("Successfully sent FCM message: {Response}", response);

            return Result.Success;
        }
        catch (FirebaseMessagingException ex)
        {
            logger.LogError(ex, "Firebase messaging error: {ErrorCode}", ex.MessagingErrorCode);

            if (ex.MessagingErrorCode == MessagingErrorCode.Unregistered ||
                ex.MessagingErrorCode == MessagingErrorCode.InvalidArgument)
            {
                // TODO(Optional): Handle failure
                /*
                    MessagingErrorCode.Unregistered:
                    - User uninstalled app
                    - User cleared app data
                    - Device token expired
                    - What to do: Mark device as inactive in database

                    MessagingErrorCode.InvalidArgument:
                    - Device token format is wrong
                    - Token is corrupted
                    - What to do: Remove from database
                */
                logger.LogWarning("Invalid or unregistered device token: {Token}", deviceToken);
            }

            return NotificationErrors.SendFailed;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while sending FCM notification");
            return NotificationErrors.SendFailed;
        }
    }

    public async Task<Result> SendToDevicesAsync(
        IReadOnlyList<string> deviceTokens,
        string title,
        string body,
        Dictionary<string, string>? data = null,
        CancellationToken cancellationToken = default)
    {
        var tokenList = deviceTokens;
        if (tokenList.Count == 0)
            return Result.Success;

        try
        {
            var message = new MulticastMessage
            {
                Tokens = tokenList,
                Notification = new FirebaseAdmin.Messaging.Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data ?? [],
                Android = new AndroidConfig
                {
                    Priority = Priority.High,
                    Notification = new AndroidNotification
                    {
                        Sound = "default",
                        ChannelId = "default"
                    }
                }
            };

            var response = await messaging.SendEachForMulticastAsync(message, cancellationToken);

            logger.LogInformation(
                "Multicast message sent. Success: {SuccessCount}, Failure: {FailureCount}",
                response.SuccessCount,
                response.FailureCount);

            if (response.FailureCount <= 0)
                return Result.Success;

            for (int i = 0; i < response.Responses.Count; i++)
            {
                if (response.Responses[i].IsSuccess)
                    continue;

                logger.LogWarning(
                    "Failed to send to token {Token}: {Error}",
                    tokenList[i],
                    response.Responses[i].Exception?.Message);
            }

            return response.SuccessCount > 0
                ? Result.Success
                : NotificationErrors.SendFailed;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred while sending multicast FCM notification");
            return NotificationErrors.SendFailed;
        }
    }
}