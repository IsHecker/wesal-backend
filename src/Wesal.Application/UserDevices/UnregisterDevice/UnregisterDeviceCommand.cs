using Wesal.Application.Messaging;

namespace Wesal.Application.UserDevices.UnregisterDevice;

public record struct UnregisterDeviceCommand(Guid UserId, string DeviceToken) : ICommand;