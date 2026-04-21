namespace Wesal.Contracts.Visitations;

public record struct VisitationResponse(
    Guid Id,
    Guid FamilyId,
    Guid NonCustodialParentId,
    string NonCustodialNationalId,
    string CompanionNationalId,
    Guid LocationId,
    Guid VisitationScheduleId,
    DateTime StartAt,
    DateTime EndAt,
    string Status,
    AttendanceResponse Attendance);

public record struct AttendanceResponse(
    DateTime? NonCustodialCheckedInAt,
    DateTime? CompanionCheckedInAt,
    DateTime? NonCustodialCheckedOutAt,
    DateTime? CompanionCheckedOutAt,
    DateTime? CompletedAt,
    bool NonCustodialOverstayed,
    bool CompanionOverstayed,
    bool IsNonCustodialCheckedIn,
    bool IsCompanionCheckedIn,
    bool IsNonCustodialCheckedOut,
    bool IsCompanionCheckedOut,
    bool AreBothCheckedIn,
    bool AreBothCheckedOut,
    IEnumerable<Guid> AttendedChildrenIds);