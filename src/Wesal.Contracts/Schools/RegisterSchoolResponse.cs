namespace Wesal.Contracts.Schools;

public record struct RegisterSchoolResponse(
    Guid SchoolId,
    string Username,
    string TemporaryPassword);