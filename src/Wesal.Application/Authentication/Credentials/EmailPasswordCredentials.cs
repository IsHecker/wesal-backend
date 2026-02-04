using Wesal.Application.Abstractions.Authentication;

namespace Wesal.Application.Authentication.Credentials;

public record struct EmailPasswordCredentials(string Email, string Password)
    : IAuthenticationCredentials;