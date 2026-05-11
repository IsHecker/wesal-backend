using Wesal.Application.Messaging;

namespace Wesal.Application.FamilyCourts.DeleteFamilyCourt;

public sealed record DeleteFamilyCourtCommand(Guid CourtId) : ICommand;