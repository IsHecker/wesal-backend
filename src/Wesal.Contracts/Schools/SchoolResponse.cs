namespace Wesal.Contracts.Schools;

public record struct SchoolResponse(
    Guid Id,
    string Name,
    string Address,
    string Governorate,
    string? Email,
    string? ContactNumber);