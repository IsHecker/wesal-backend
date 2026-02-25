using Wesal.Application.Messaging;

namespace Wesal.Application.VisitationLocations.CreateVisitationLocation;

public record struct CreateVisitationLocationCommand(
    Guid StaffId,
    Guid CourtId,
    string Name,
    string Address,
    string? ContactNumber,
    int MaxConcurrentVisits,
    TimeOnly OpeningTime,
    TimeOnly ClosingTime) : ICommand<Guid>;