using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Users;
using Wesal.Domain.Entities.SystemAdmins;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.SystemAdmins.CreateSystemAdmin;

internal sealed class CreateSystemAdminCommandHandler(
    IRepository<SystemAdmin> systemAdminRepository,
    IUserService userService)
    : ICommandHandler<CreateSystemAdminCommand, UserCredentialResponse>
{
    public async Task<Result<UserCredentialResponse>> Handle(
        CreateSystemAdminCommand request,
        CancellationToken cancellationToken)
    {
        var isExisting = await userService.ExistsByEmailAsync<SystemAdmin>(
            request.Email,
            cancellationToken);

        if (isExisting)
            return UserErrors.EmailAlreadyExists(request.Email);

        var userResult = await userService.CreateAsync(
            UserRole.SystemAdmin,
            cancellationToken);

        var systemAdmin = SystemAdmin.Create(
            userResult.User.Id,
            request.Email,
            request.FullName);

        await systemAdminRepository.AddAsync(systemAdmin, cancellationToken);

        return new UserCredentialResponse(systemAdmin.Id, systemAdmin.FullName, userResult.TemporaryPassword);
    }
}