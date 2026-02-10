namespace Wesal.Application.Families.EnrollFamily.Dtos;

public record struct CreateParentDto(
    string NationalId,
    string FullName,
    DateOnly BirthDate,
    string Gender,
    string Address,
    string Phone,
    string? Job,
    string? Email);