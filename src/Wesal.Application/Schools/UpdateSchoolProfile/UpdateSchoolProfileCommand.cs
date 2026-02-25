using Wesal.Application.Messaging;

namespace Wesal.Application.Schools.UpdateSchoolProfile;

public record struct UpdateSchoolProfileCommand(
    Guid SchoolId,
    string? Email,
    string? ContactNumber) : ICommand;