using Wesal.Application.Messaging;
using Wesal.Contracts.Users;

namespace Wesal.Application.SystemAdmins.CreateSystemAdmin;

public record struct CreateSystemAdminCommand(string Email, string FullName) : ICommand<UserCredentialResponse>;