using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Messaging;
using Wesal.Contracts.Users;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtStaffs.CreateCourtStaff;

internal sealed class CreateCourtStaffCommandHandler(
    ICourtStaffRepository courtStaffRepository,
    IUserService userService)
    : ICommandHandler<CreateCourtStaffCommand, UserCredentialResponse>
{
    public async Task<Result<UserCredentialResponse>> Handle(
        CreateCourtStaffCommand request,
        CancellationToken cancellationToken)
    {
        var isExisting = await userService.ExistsByEmailAsync<CourtStaff>(
            request.Email,
            cancellationToken);

        if (isExisting)
            return UserErrors.EmailAlreadyExists(request.Email);

        var userResult = await userService.CreateAsync(UserRole.CourtStaff, cancellationToken);

        var courtStaff = CourtStaff.Create(
            userResult.User.Id,
            request.CourtId,
            request.Email,
            request.FullName,
            request.Phone);

        await courtStaffRepository.AddAsync(courtStaff, cancellationToken);

        return new UserCredentialResponse(courtStaff.Id, courtStaff.FullName, userResult.TemporaryPassword);
    }
}