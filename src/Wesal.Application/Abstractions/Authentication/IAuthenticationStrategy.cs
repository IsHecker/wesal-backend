using Wesal.Contracts.Authentication;
using Wesal.Domain.Results;

namespace Wesal.Application.Abstractions.Authentication;

public interface IAuthenticationStrategy<TCredentials>
    where TCredentials : IAuthenticationCredentials
{
    Task<Result<JwtTokenResponse>> AuthenticateAsync(
        TCredentials credentials,
        string userRole,
        CancellationToken cancellationToken = default);
}