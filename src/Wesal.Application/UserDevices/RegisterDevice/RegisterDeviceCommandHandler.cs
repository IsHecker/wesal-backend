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
        var existingDevice = await deviceRepository
            .GetByUserIdAndTokenAsync(request.ParentId, request.DeviceToken, cancellationToken);

        if (existingDevice is null)
        {
            var device = UserDevice.Create(
                request.ParentId,
                request.DeviceToken,
                request.Platform.ToEnum<DevicePlatform>());

            await deviceRepository.AddAsync(device, cancellationToken);

            return device.Id;
        }

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