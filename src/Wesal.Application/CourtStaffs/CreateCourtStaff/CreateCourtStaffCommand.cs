using Wesal.Application.Messaging;
using Wesal.Contracts.Users;

namespace Wesal.Application.CourtStaffs.CreateCourtStaff;

public record struct CreateCourtStaffCommand(
    Guid CourtId,
    string Email,
    string FullName,
    string? Phone) : ICommand<UserCredentialResponse>;