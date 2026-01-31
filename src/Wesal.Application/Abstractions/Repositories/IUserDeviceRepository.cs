using Wesal.Application.Data;
using Wesal.Domain.Entities.UserDevices;

namespace Wesal.Application.Abstractions.Repositories;

public interface IUserDeviceRepository : IRepository<UserDevice>
{
    Task<UserDevice?> GetByUserIdAndTokenAsync(Guid userId, string token, CancellationToken cancellationToken = default);
}