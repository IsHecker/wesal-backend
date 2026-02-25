using Wesal.Application.Messaging;
using Wesal.Contracts.VisitCenterStaffs;

namespace Wesal.Application.VisitCenterStaffs.GetVisitCenterStaffProfile;

public sealed record GetVisitCenterStaffProfileQuery(Guid CenterStaffId)
    : IQuery<VisitCenterStaffResponse>;