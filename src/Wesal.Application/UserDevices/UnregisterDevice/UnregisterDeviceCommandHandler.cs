using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Results;

namespace Wesal.Application.UserDevices.UnregisterDevice;

internal sealed class UnregisterDeviceCommandHandler(
    IUserDeviceRepository deviceRepository)
    : ICommandHandler<UnregisterDeviceCommand>
{
    public async Task<Result> Handle(
        UnregisterDeviceCommand request,
        CancellationToken cancellationToken)
    {
        var device = await deviceRepository.GetByUserIdAndTokenAsync(
            request.ParentId,
            request.DeviceToken,
            cancellationToken);

        if (device is null)
            return Result.Success;

        device.Deactivate();
        deviceRepository.Update(device);

        return Result.Success;
    }
}