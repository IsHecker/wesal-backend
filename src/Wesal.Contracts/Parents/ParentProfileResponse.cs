namespace Wesal.Contracts.Parents;

public record struct ParentProfileResponse(
    Guid Id,
    string FullName,
    string NationalId,
    DateOnly BirthDate,
    string Gender,
    string? Job,
    string? Address,
    string? Phone,
    string? Email);