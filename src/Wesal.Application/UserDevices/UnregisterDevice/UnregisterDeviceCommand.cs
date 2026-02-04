using Wesal.Application.Messaging;

namespace Wesal.Application.UserDevices.UnregisterDevice;

public record struct UnregisterDeviceCommand(Guid ParentId, string DeviceToken) : ICommand;