namespace Wesal.Contracts.Alimonies;

public record struct AlimonyResponse(
    Guid Id,
    Guid CourtId,
    Guid CourtCaseId,
    Guid PayerId,
    Guid RecipientId,
    Guid FamilyId,
    long Amount,
    string Frequency,
    DateOnly StartDate,
    DateOnly? EndDate);