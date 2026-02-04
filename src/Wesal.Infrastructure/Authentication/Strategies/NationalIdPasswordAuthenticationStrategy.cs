using Wesal.Application.Abstractions.Authentication;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Authentication.Credentials;
using Wesal.Contracts.Authentication;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;
using Wesal.Infrastructure.Authentication.Services;

namespace Wesal.Infrastructure.Authentication.Strategies;

internal sealed class NationalIdPasswordAuthenticationStrategy(
    IParentRepository parentRepository,
    AuthenticationService authenticationService)
    : IAuthenticationStrategy<NationalIdPasswordCredentials>
{
    public async Task<Result<JwtTokenResponse>> AuthenticateAsync(
        NationalIdPasswordCredentials credentials,
        string userRole,
        CancellationToken cancellationToken = default)
    {
        var parent = await parentRepository.GetByNationalIdAsync(credentials.NationalId, cancellationToken);

        if (parent is null)
            return UserErrors.InvalidCredentials;

        return await authenticationService.AuthenticateAsync(
            parent.UserId,
            credentials.Password,
            parent.Id,
            cancellationToken: cancellationToken);
    }
}