using Wesal.Application.Abstractions.Authentication;

namespace Wesal.Application.Authentication.Credentials;

public record struct UsernamePasswordCredentials(string Username, string Password)
    : IAuthenticationCredentials;