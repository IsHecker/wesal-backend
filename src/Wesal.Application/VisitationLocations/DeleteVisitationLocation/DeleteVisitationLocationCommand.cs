using Wesal.Application.Messaging;

namespace Wesal.Application.VisitationLocations.DeleteVisitationLocation;

public record struct DeleteVisitationLocationCommand(Guid CourtId, Guid LocationId) : ICommand;