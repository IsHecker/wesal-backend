using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.CourtStaffs;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtStaffs.GetCourtStaffProfile;

internal sealed class GetCourtStaffProfileQueryHandler(
    ICourtStaffRepository courtStaffRepository)
    : IQueryHandler<GetCourtStaffProfileQuery, CourtStaffResponse>
{
    public async Task<Result<CourtStaffResponse>> Handle(
        GetCourtStaffProfileQuery request,
        CancellationToken cancellationToken)
    {
        var staff = await courtStaffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return CourtStaffErrors.NotFound(request.StaffId);

        return new CourtStaffResponse(
            staff.Id,
            staff.CourtId,
            staff.FullName,
            staff.Email,
            staff.Phone);
    }
}