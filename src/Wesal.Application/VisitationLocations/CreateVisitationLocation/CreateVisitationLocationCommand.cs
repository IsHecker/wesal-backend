using Wesal.Application.Messaging;

namespace Wesal.Application.VisitationLocations.CreateVisitationLocation;

public record struct CreateVisitationLocationCommand(
    Guid StaffId,
    string Name,
    string Address,
    string Governorate,
    string? ContactNumber,
    int MaxConcurrentVisits,
    TimeOnly OpeningTime,
    TimeOnly ClosingTime) : ICommand<Guid>;