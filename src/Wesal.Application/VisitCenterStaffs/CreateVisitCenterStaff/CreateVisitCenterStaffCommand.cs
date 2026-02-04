using Wesal.Application.Messaging;
using Wesal.Contracts.Users;

namespace Wesal.Application.VisitCenterStaffs.CreateVisitCenterStaff;

public record struct CreateVisitCenterStaffCommand(
    Guid CourtId,
    Guid LocationId,
    string Email,
    string FullName,
    string? Phone) : ICommand<UserCredentialResponse>;