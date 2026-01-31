using Wesal.Application.Messaging;
using Wesal.Domain.Entities.UserDevices;

namespace Wesal.Application.UserDevices.RegisterDevice;

public record struct RegisterDeviceCommand(
    Guid UserId,
    string DeviceToken,
    string Platform) : ICommand<Guid>;