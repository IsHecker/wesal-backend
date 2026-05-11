using Wesal.Application.Messaging;

namespace Wesal.Application.VisitCenterStaffs.UpdateVisitCenterStaff;

public record struct UpdateVisitCenterStaffCommand(
    Guid CourtId,
    Guid StaffId,
    string FullName,
    string? Phone) : ICommand;