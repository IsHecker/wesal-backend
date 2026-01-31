using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.UserDevices;
using Wesal.Domain.Results;

namespace Wesal.Application.UserDevices.RegisterDevice;

internal sealed class RegisterDeviceCommandHandler(
    IUserDeviceRepository deviceRepository)
    : ICommandHandler<RegisterDeviceCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterDeviceCommand request,
        CancellationToken cancellationToken)
    {
        // Check if device token already exists for this user
        var existingDevice = await deviceRepository
            .GetByUserIdAndTokenAsync(request.UserId, request.DeviceToken, cancellationToken);

        if (existingDevice is null)
        {
            var device = UserDevice.Create(
                request.UserId,
                request.DeviceToken,
                request.Platform.ToEnum<DevicePlatform>());

            await deviceRepository.AddAsync(device, cancellationToken);

            return device.Id;
        }

        // Reactivate if it was deactivated
        if (!existingDevice.IsActive)
        {
            existingDevice.Activate();
            deviceRepository.Update(existingDevice);
        }
        else
        {
            existingDevice.UpdateLastUsed();
            deviceRepository.Update(existingDevice);
        }

        return existingDevice.Id;
    }
}
