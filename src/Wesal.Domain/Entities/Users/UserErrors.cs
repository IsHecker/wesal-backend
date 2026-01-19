using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Users;

public class UserErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "User.NotFound",
            $"User with ID '{id}' was not found");
}