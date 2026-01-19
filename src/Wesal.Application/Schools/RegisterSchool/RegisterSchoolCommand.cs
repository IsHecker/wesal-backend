using Wesal.Application.Messaging;
using Wesal.Contracts.Schools;

namespace Wesal.Application.Schools.RegisterSchool;

public record struct RegisterSchoolCommand(
    Guid UserId,
    string Name,
    string Address,
    string? ContactNumber = null) : ICommand<RegisterSchoolResponse>;