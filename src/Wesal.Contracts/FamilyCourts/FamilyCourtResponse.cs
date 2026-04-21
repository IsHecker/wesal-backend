namespace Wesal.Contracts.FamilyCourts;

public record FamilyCourtResponse(
    Guid Id,
    string Email,
    string Name,
    string Governorate,
    string Address,
    string ContactInfo);