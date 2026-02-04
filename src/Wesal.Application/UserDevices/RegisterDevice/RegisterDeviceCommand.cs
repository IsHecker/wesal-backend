using Wesal.Application.Messaging;

namespace Wesal.Application.UserDevices.RegisterDevice;

public record struct RegisterDeviceCommand(
    Guid ParentId,
    string DeviceToken,
    string Platform) : ICommand<Guid>;