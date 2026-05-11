using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Authentication;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Authentication.Credentials;
using Wesal.Contracts.Authentication;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;
using Wesal.Infrastructure.Authentication.Services;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Authentication.Strategies;

internal sealed class NationalIdPasswordAuthenticationStrategy(
    IParentRepository parentRepository,
    WesalDbContext dbContext,
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

        var custody = await dbContext.Custodies
            .Where(c => c.CustodialParentId == parent.Id || c.NonCustodialParentId == parent.Id)
            .OrderByDescending(c => c.StartAt)
            .FirstOrDefaultAsync(cancellationToken);

        return await authenticationService.AuthenticateAsync(
            parent.UserId,
            credentials.Password,
            parent.FullName,
            parent.Id,
            parent.CourtId,
            parent.IsFather,
            isCustodialParent: custody?.CustodialParentId == parent.Id,
            cancellationToken: cancellationToken);
    }
}