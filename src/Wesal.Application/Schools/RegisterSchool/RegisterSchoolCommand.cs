using Wesal.Application.Messaging;
using Wesal.Contracts.Schools;

namespace Wesal.Application.Schools.RegisterSchool;

public record struct RegisterSchoolCommand(
    string Name,
    string Address,
    string Governorate,
    string? ContactNumber = null) : ICommand<RegisterSchoolResponse>;