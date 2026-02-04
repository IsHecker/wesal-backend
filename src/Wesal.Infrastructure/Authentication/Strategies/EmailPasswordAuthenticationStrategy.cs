using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Authentication;
using Wesal.Application.Authentication.Credentials;
using Wesal.Contracts.Authentication;
using Wesal.Domain.Common.Abstractions;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.SystemAdmins;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Domain.Results;
using Wesal.Infrastructure.Authentication.Services;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Authentication.Strategies;

internal sealed class EmailPasswordAuthenticationStrategy(
    WesalDbContext dbContext,
    AuthenticationService authenticationService) : IAuthenticationStrategy<EmailPasswordCredentials>
{
    public async Task<Result<JwtTokenResponse>> AuthenticateAsync(
        EmailPasswordCredentials credentials,
        string userRole,
        CancellationToken cancellationToken = default)
    {
        var authResult = userRole switch
        {
            UserRole.SystemAdmin => await AuthenticateRoleAsync<SystemAdmin>(
                credentials,
                admin => null,
                cancellationToken),

            UserRole.FamilyCourt => await AuthenticateRoleAsync<FamilyCourt>(
                credentials,
                court => court.Id,
                cancellationToken),

            UserRole.CourtStaff => await AuthenticateRoleAsync<CourtStaff>(
                credentials,
                staff => staff.CourtId,
                cancellationToken),

            UserRole.VisitCenterStaff => await AuthenticateRoleAsync<VisitCenterStaff>(
                credentials,
                staff => staff.CourtId,
                cancellationToken),

            _ => throw new InvalidOperationException()
        };

        if (authResult.IsFailure)
            return authResult.Error;

        return await authenticationService.AuthenticateAsync(
            authResult.Value.userId,
            credentials.Password,
            authResult.Value.roleId,
            authResult.Value.courtId,
            cancellationToken);
    }

    private async Task<Result<(Guid userId, Guid roleId, Guid? courtId)>> AuthenticateRoleAsync<TEntity>(
        EmailPasswordCredentials credentials,
        Func<TEntity, Guid?> courtIdSelector,
        CancellationToken cancellationToken) where TEntity : Entity, IHasUserId
    {
        var entity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(
            e => EF.Property<string>(e, "Email") == credentials.Email,
            cancellationToken);

        if (entity is null)
            return UserErrors.InvalidCredentials;

        return (entity.UserId, entity.Id, courtIdSelector(entity));
    }
}