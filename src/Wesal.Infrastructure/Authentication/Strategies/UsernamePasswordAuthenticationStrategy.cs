using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Authentication;
using Wesal.Application.Authentication.Credentials;
using Wesal.Contracts.Authentication;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;
using Wesal.Infrastructure.Authentication.Services;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Authentication.Strategies;

internal sealed class UsernamePasswordAuthenticationStrategy(
    WesalDbContext dbContext,
    AuthenticationService authenticationService)
    : IAuthenticationStrategy<UsernamePasswordCredentials>
{
    public async Task<Result<JwtTokenResponse>> AuthenticateAsync(
        UsernamePasswordCredentials credentials,
        string userRole,
        CancellationToken cancellationToken = default)
    {
        var school = await GetSchoolByUsernameAsync(credentials.Username, cancellationToken);

        if (school is null)
            return UserErrors.InvalidCredentials;

        return await authenticationService.AuthenticateAsync(
            school.UserId,
            credentials.Password,
            school.Name,
            school.Id,
            cancellationToken: cancellationToken);
    }

    private Task<School?> GetSchoolByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return dbContext.Schools
            .FirstOrDefaultAsync(school => school.Username == username, cancellationToken);
    }
}