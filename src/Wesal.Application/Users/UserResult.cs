using Wesal.Domain.Entities.Users;

namespace Wesal.Application.Users;

public record struct UserResult(User User, string TemporaryPassword);