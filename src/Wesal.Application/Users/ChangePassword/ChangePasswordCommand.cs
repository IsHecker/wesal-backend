using Wesal.Application.Messaging;

namespace Wesal.Application.Users.ChangePassword;

public record struct ChangePasswordCommand(
    Guid UserId,
    string OldPassword,
    string NewPassword) : ICommand;