using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wesal.Application.Authentication;
using Wesal.Application.Exceptions;

namespace Wesal.Presentation.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        string? userId = principal?.FindFirstValue(JwtRegisteredClaimNames.Sub)!;

        return Guid.TryParse(userId, out Guid parsedUserId) ?
            parsedUserId :
            throw new WesalException("User identifier is unavailable");
    }

    public static Guid GetRoleId(this ClaimsPrincipal? principal)
    {
        string? roleId = principal?.FindFirstValue(CustomClaims.RoleId)!;

        return Guid.TryParse(roleId, out Guid parsedRoleId) ?
            parsedRoleId :
            throw new WesalException("Role identifier is unavailable");
    }

    public static string GetRole(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(CustomClaims.Role)!;
    }

    public static Guid GetCourtId(this ClaimsPrincipal? principal)
    {
        string? courtId = principal?.FindFirstValue(CustomClaims.CourtId)!;

        return Guid.TryParse(courtId, out Guid parsedCourtId) ?
            parsedCourtId :
            throw new WesalException("Court identifier is unavailable");
    }
}