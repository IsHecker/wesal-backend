using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Notifications;

public sealed class Notification : Entity
{
    public Guid RecipientId { get; private set; }

    public string Content { get; private set; } = null!;
    public NotificationType Type { get; private set; }
    public NotificationStatus Status { get; private set; }

    public DateTime SentAt { get; private set; }
    public DateTime? ReadAt { get; private set; }

    private Notification() { }

    public static Notification Create(
        Guid recipientId,
        string content,
        NotificationType type,
        NotificationStatus status,
        DateTime sentAt,
        DateTime? readAt = null)
    {
        return new Notification
        {
            RecipientId = recipientId,
            Content = content,
            Type = type,
            Status = status,
            SentAt = sentAt,
            ReadAt = readAt,
        };
    }
}