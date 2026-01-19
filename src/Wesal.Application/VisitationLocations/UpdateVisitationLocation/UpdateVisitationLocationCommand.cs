using Wesal.Application.Messaging;

namespace Wesal.Application.VisitationLocations.UpdateVisitationLocation;

public record struct UpdateVisitationLocationCommand(
    Guid LocationId,
    Guid UserId,
    string Name,
    string Address,
    string Governorate,
    string? ContactNumber,
    int MaxConcurrentVisits,
    TimeOnly OpeningTime,
    TimeOnly ClosingTime) : ICommand;