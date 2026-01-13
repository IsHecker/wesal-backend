namespace Wesal.Application.Families.EnrollFamily.Dtos;

public record struct CreateChildDto(
    string FullName,
    DateTime BirthDate,
    string Gender,
    Guid? SchoolId);