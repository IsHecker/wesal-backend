using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Wesal.Application.Authentication;
using Wesal.Contracts.Authentication;
using Wesal.Domain.Entities.Users;

namespace Wesal.Infrastructure.Authentication;

public class TokenGeneratorService(JwtOptions options)
{
    public JwtTokenResponse GenerateToken(User user, Guid roleId, Guid? courtId = null, bool? isFather = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(options.SecretKey);
        var expiresIn = DateTimeOffset.UtcNow.AddMinutes(options.ExpiryMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(CustomClaims.RoleId, roleId.ToString()),
            new(CustomClaims.Role, user.Role.ToString()),
            new(CustomClaims.PasswordChangeRequired, user.PasswordChangeRequired.ToString()),
            new(JwtRegisteredClaimNames.Iat,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                ClaimValueTypes.Integer64)
        };

        if (courtId.HasValue)
            claims.Add(new(CustomClaims.CourtId, courtId.Value.ToString()));

        if (isFather.HasValue)
            claims.Add(new(CustomClaims.ParentRole, isFather.Value.ToString()));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresIn.DateTime,
            Issuer = options.Issuer,
            Audience = options.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new JwtTokenResponse(
            tokenHandler.WriteToken(token),
            expiresIn.ToUnixTimeMilliseconds(),
            "refresh token");
    }
}