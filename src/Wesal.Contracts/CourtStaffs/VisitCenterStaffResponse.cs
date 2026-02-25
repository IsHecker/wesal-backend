namespace Wesal.Contracts.CourtStaffs;

public record struct CourtStaffResponse(
    Guid Id,
    Guid CourtId,
    string FullName,
    string Email,
    string? Phone);