namespace Wesal.Contracts.Notifications;

public record struct NotificationResponse(
    Guid Id,
    string Content,
    string Type,
    string Status,
    DateTime SentAt,
    DateTime? ReadAt);