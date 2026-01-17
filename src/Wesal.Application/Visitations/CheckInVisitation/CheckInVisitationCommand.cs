using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.CheckInVisitation;

public record struct CheckInVisitationCommand(Guid VisitationId, Guid StaffId) : ICommand;