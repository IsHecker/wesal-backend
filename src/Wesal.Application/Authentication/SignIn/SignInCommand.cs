using Wesal.Application.Abstractions.Authentication;
using Wesal.Application.Messaging;
using Wesal.Contracts.Authentication;

namespace Wesal.Application.Authentication.SignIn;

public record struct SignInCommand<TCredentials>(TCredentials Credentials, string UserRole)
    : ICommand<JwtTokenResponse> where TCredentials : IAuthenticationCredentials;