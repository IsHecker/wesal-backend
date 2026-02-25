using Wesal.Application.Messaging;
using Wesal.Contracts.CourtStaffs;

namespace Wesal.Application.CourtStaffs.GetCourtStaffProfile;

public record struct GetCourtStaffProfileQuery(Guid StaffId) : IQuery<CourtStaffResponse>;