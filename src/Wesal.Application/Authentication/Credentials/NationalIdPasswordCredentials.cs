using Wesal.Application.Abstractions.Authentication;

namespace Wesal.Application.Authentication.Credentials;

public record struct NationalIdPasswordCredentials(string NationalId, string Password)
    : IAuthenticationCredentials;