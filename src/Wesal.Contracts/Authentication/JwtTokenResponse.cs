namespace Wesal.Contracts.Authentication;

public record struct JwtTokenResponse(string Token, long ExpiresInSeconds, string? RefreshToken);