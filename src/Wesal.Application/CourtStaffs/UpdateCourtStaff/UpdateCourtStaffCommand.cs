using Wesal.Application.Messaging;

namespace Wesal.Application.CourtStaffs.UpdateCourtStaff;

public record struct UpdateCourtStaffCommand(
    Guid CourtId,
    Guid StaffId,
    string FullName,
    string? Phone) : ICommand;