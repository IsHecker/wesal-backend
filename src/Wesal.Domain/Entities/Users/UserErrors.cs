using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.Users;

public class UserErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "User.NotFound",
            $"User with ID '{id}' was not found");

    public static readonly Error InvalidCredentials =
        Error.Validation(
            "Users.InvalidCredentials",
            "The provided credentials are invalid");

    public static readonly Error UserNotActive =
        Error.Validation(
            "Users.NotActive",
            "This user account is not active");

    public static Error EmailAlreadyExists(string email) =>
        Error.Conflict(
            "Users.EmailAlreadyExists",
            $"User with email '{email}' already exists");

    public static Error NationalIdAlreadyExists(string nationalId) =>
        Error.Conflict(
            "Users.NationalIdAlreadyExists",
            $"User with national ID '{nationalId}' already exists");

    public static readonly Error UnauthorizedAccess =
        Error.Forbidden(
            "Users.UnauthorizedAccess",
            "You are not authorized to perform this action");

    public static readonly Error InvalidPassword =
        Error.Validation(
            "Users.InvalidPassword",
            "The provided password does not meet security requirements");

    public static readonly Error IncorrectOldPassword =
        Error.Validation(
            "Users.IncorrectOldPassword",
            "The old password is incorrect");

    public static Error RoleEntityNotFound(string entityType, Guid id) =>
        Error.NotFound(
            "Users.RoleEntityNotFound",
            $"{entityType} with ID '{id}' was not found");

    public static Error CrossCourtAccessDenied =>
        Error.Forbidden(
            "Users.CrossCourtAccessDenied",
            $"You cannot access resources from different court");
}