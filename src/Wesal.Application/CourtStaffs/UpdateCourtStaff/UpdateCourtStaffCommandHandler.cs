using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtStaffs.UpdateCourtStaff;

internal sealed class UpdateCourtStaffCommandHandler(ICourtStaffRepository courtStaffRepository)
    : ICommandHandler<UpdateCourtStaffCommand>
{
    public async Task<Result> Handle(
        UpdateCourtStaffCommand request,
        CancellationToken cancellationToken)
    {
        var staff = await courtStaffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null || staff.CourtId != request.CourtId)
            return Error.NotFound("CourtStaff.NotFound", "Court staff member not found.");

        staff.UpdateProfile(request.FullName, request.Phone);

        courtStaffRepository.Update(staff);

        return Result.Success;
    }
}