using Wesal.Application.Messaging;
using Wesal.Contracts.Users;

namespace Wesal.Application.Schools.RegisterSchool;

public record struct RegisterSchoolCommand(
    Guid CourtId,
    string Name,
    string Address,
    string? ContactNumber = null) : ICommand<UserCredentialResponse>;