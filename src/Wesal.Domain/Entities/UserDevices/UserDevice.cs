using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.UserDevices;

/// <summary>
/// Represents a user's registered device for push notifications
/// </summary>
public sealed class UserDevice : Entity
{
    public Guid UserId { get; private set; }
    public string DeviceToken { get; private set; } = null!;
    public DevicePlatform Platform { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public DateTime LastUsedAt { get; private set; }
    public bool IsActive { get; private set; }

    private UserDevice() { }

    public static UserDevice Create(
        Guid userId,
        string deviceToken,
        DevicePlatform platform)
    {
        return new UserDevice
        {
            UserId = userId,
            DeviceToken = deviceToken,
            Platform = platform,
            RegisteredAt = DateTime.UtcNow,
            LastUsedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void UpdateLastUsed() => LastUsedAt = DateTime.UtcNow;

    public void Deactivate() => IsActive = false;

    public void Activate()
    {
        IsActive = true;
        LastUsedAt = DateTime.UtcNow;
    }
}