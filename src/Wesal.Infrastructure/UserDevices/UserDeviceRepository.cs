using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.UserDevices;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.UserDevices;

internal sealed class UserDeviceRepository(WesalDbContext context)
    : Repository<UserDevice>(context), IUserDeviceRepository
{
    public Task<UserDevice?> GetByUserIdAndTokenAsync(
        Guid userId,
        string deviceToken,
        CancellationToken cancellationToken = default)
    {
        return context.UserDevices
            .FirstOrDefaultAsync(device =>
                device.UserId == userId && device.DeviceToken == deviceToken, cancellationToken);
    }
}