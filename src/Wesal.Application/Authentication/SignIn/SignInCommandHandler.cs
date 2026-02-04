using Wesal.Application.Abstractions.Authentication;
using Wesal.Application.Messaging;
using Wesal.Contracts.Authentication;
using Wesal.Domain.Results;

namespace Wesal.Application.Authentication.SignIn;

internal sealed class SignInCommandHandler<TCredentials>(
    IAuthenticationStrategy<TCredentials> strategy)
    : ICommandHandler<SignInCommand<TCredentials>, JwtTokenResponse>
    where TCredentials : IAuthenticationCredentials
{
    public async Task<Result<JwtTokenResponse>> Handle(
        SignInCommand<TCredentials> request,
        CancellationToken cancellationToken)
    {
        return await strategy.AuthenticateAsync(
            request.Credentials,
            request.UserRole,
            cancellationToken);
    }
}