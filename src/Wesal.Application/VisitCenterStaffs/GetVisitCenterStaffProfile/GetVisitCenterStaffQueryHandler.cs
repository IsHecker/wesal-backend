using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.VisitCenterStaffs;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitCenterStaffs.GetVisitCenterStaffProfile;

internal sealed class GetVisitCenterStaffQueryHandler(IVisitCenterStaffRepository staffRepository)
    : IQueryHandler<GetVisitCenterStaffProfileQuery, VisitCenterStaffResponse>
{
    public async Task<Result<VisitCenterStaffResponse>> Handle(
        GetVisitCenterStaffProfileQuery request,
        CancellationToken cancellationToken)
    {
        var centerStaff = await staffRepository.GetByIdAsync(request.CenterStaffId, cancellationToken);
        if (centerStaff is null)
            return VisitCenterStaffErrors.NotFound(request.CenterStaffId);

        return centerStaff.ToResponse();
    }
}