using Microsoft.AspNetCore.Identity;
using Wesal.Application.Data;
using Wesal.Contracts.Authentication;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.Authentication.Services;

internal sealed class AuthenticationService(
    IRepository<User> userRepository,
    IPasswordHasher<User> passwordHasher,
    TokenGeneratorService tokenGenerator)
{
    public async Task<Result<JwtTokenResponse>> AuthenticateAsync(
        Guid userId,
        string password,
        Guid roleId,
        Guid? courtId = null,
        bool? isFather = null,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return UserErrors.NotFound(userId);

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            return UserErrors.InvalidCredentials;

        return tokenGenerator.GenerateToken(user, roleId, courtId, isFather);
    }
}