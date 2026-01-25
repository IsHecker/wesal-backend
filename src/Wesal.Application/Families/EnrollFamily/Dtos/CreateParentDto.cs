namespace Wesal.Application.Families.EnrollFamily.Dtos;

public record struct CreateParentDto(
    string NationalId,
    string FullName,
    DateOnly BirthDate,
    string Gender,
    string? Job,
    string? Address,
    string? Phone,
    string? Email);