using Wesal.Domain.Common;
using Wesal.Domain.Common.Abstractions;
using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.UserDevices;

public sealed class UserDevice : Entity, IHasUserId
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
            RegisteredAt = EgyptTime.Now,
            LastUsedAt = EgyptTime.Now,
            IsActive = true
        };
    }

    public void UpdateLastUsed() => LastUsedAt = EgyptTime.Now;

    public void Deactivate() => IsActive = false;

    public void Activate()
    {
        IsActive = true;
        LastUsedAt = EgyptTime.Now;
    }
}