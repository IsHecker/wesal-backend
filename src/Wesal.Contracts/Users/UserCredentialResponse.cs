namespace Wesal.Contracts.Users;

public record struct UserCredentialResponse(
    Guid UserId,
    string Username,
    string TemporaryPassword);