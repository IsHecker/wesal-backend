using Wesal.Application.Messaging;
using Wesal.Contracts.VisitationLocations;

namespace Wesal.Application.VisitationLocations.GetVisitationLocation;

public record struct GetVisitationLocationQuery(Guid LocationId)
    : IQuery<VisitationLocationResponse>;