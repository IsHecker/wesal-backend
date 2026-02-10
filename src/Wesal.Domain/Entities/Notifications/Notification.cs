using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.Notifications;

public sealed class Notification : Entity
{
    public Guid RecipientId { get; private set; }

    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public NotificationType Type { get; private set; }
    public NotificationStatus Status { get; private set; }

    public DateTime SentAt { get; private set; }
    public DateTime? ReadAt { get; private set; }

    private Notification() { }

    public static Notification Create(
        Guid recipientId,
        string title,
        string content,
        NotificationType type)
    {
        return new Notification
        {
            RecipientId = recipientId,
            Title = title,
            Content = content,
            Type = type,
            Status = NotificationStatus.Sent,
            SentAt = EgyptTime.Now,
            ReadAt = null
        };
    }

    public void MarkAsRead()
    {
        if (Status == NotificationStatus.Read)
            return;

        Status = NotificationStatus.Read;
        ReadAt = EgyptTime.Now;
    }
}