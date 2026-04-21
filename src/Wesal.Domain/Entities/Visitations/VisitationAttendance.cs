namespace Wesal.Domain.Entities.Visitations;

public sealed class VisitationAttendance
{
    public DateTime? NonCustodialCheckedInAt { get; internal set; }
    public DateTime? CompanionCheckedInAt { get; internal set; }
    public DateTime? NonCustodialCheckedOutAt { get; internal set; }
    public DateTime? CompanionCheckedOutAt { get; internal set; }
    public DateTime? CompletedAt { get; internal set; }
    public bool NonCustodialOverstayed { get; internal set; }
    public bool CompanionOverstayed { get; internal set; }
    public List<Guid> AttendedChildrenIds { get; internal set; } = [];

    public bool IsNonCustodialCheckedIn => NonCustodialCheckedInAt.HasValue;
    public bool IsCompanionCheckedIn => CompanionCheckedInAt.HasValue;
    public bool IsNonCustodialCheckedOut => NonCustodialCheckedOutAt.HasValue;
    public bool IsCompanionCheckedOut => CompanionCheckedOutAt.HasValue;
    public bool AreBothCheckedIn => IsNonCustodialCheckedIn && IsCompanionCheckedIn;
    public bool AreBothCheckedOut => IsNonCustodialCheckedOut && IsCompanionCheckedOut;
}