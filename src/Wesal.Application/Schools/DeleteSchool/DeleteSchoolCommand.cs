using Wesal.Application.Messaging;

namespace Wesal.Application.Schools.DeleteSchool;

public record struct DeleteSchoolCommand(Guid SchoolId) : ICommand;