using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitCenterStaffs.UpdateVisitCenterStaff;

internal sealed class UpdateVisitCenterStaffCommandHandler(IVisitCenterStaffRepository visitCenterStaffRepository)
    : ICommandHandler<UpdateVisitCenterStaffCommand>
{
    public async Task<Result> Handle(
        UpdateVisitCenterStaffCommand request,
        CancellationToken cancellationToken)
    {
        var staff = await visitCenterStaffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null || staff.CourtId != request.CourtId)
            return Error.NotFound("VisitCenterStaff.NotFound", "Visit center staff member not found.");

        staff.UpdateProfile(request.FullName, request.Phone);
        visitCenterStaffRepository.Update(staff);

        return Result.Success;
    }
}