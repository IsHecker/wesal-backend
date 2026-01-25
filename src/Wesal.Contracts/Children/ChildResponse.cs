namespace Wesal.Contracts.Children;

public record struct ChildResponse(
    Guid Id,
    string FullName,
    Guid? SchoolId,
    string Gender,
    DateOnly BirthDate,
    int Age);