using Wesal.Application.Messaging;
using Wesal.Contracts.FamilyCourts;

namespace Wesal.Application.FamilyCourts.GetFamilyCourt;

public record GetFamilyCourtQuery(Guid CourtId) : IQuery<FamilyCourtResponse>;