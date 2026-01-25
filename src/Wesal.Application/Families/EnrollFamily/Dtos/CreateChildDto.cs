namespace Wesal.Application.Families.EnrollFamily.Dtos;

public record struct CreateChildDto(
    string FullName,
    DateOnly BirthDate,
    string Gender,
    Guid? SchoolId);