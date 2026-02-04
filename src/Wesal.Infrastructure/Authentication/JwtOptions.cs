namespace Wesal.Infrastructure.Options;

/// <summary>
/// Configuration options for JWT token generation
/// </summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    /// <summary>
    /// Secret key for signing tokens (minimum 256 bits)
    /// </summary>
    public string SecretKey { get; init; } = null!;

    /// <summary>
    /// Token issuer
    /// </summary>
    public string Issuer { get; init; } = null!;

    /// <summary>
    /// Token audience
    /// </summary>
    public string Audience { get; init; } = null!;

    /// <summary>
    /// Token expiry in minutes
    /// </summary>
    public int ExpiryMinutes { get; init; } = 60;
}
