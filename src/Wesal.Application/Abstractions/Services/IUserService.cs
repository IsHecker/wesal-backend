using Wesal.Application.Users;
using Wesal.Domain.DomainEvents;

namespace Wesal.Application.Abstractions.Services;

public interface IUserService
{
    Task<UserResult> CreateAsync(
        string role,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync<TEntity>(
        string email,
        CancellationToken cancellationToken) where TEntity : Entity;
}