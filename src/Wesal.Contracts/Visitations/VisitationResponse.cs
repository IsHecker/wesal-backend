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
    DateTime? NonCustodialCheckedInAt,
    DateTime? CompanionCheckedInAt,
    DateTime? CompletedAt);