namespace Wesal.Contracts.VisitCenterStaffs;

public record struct VisitCenterStaffResponse(
    Guid Id,
    Guid CourtId,
    Guid LocationId,
    string FullName,
    string Email,
    string? Phone);