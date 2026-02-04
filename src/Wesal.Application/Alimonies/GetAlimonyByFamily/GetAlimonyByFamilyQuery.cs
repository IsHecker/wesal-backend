using Wesal.Application.Messaging;
using Wesal.Contracts.Alimonies;

namespace Wesal.Application.Alimonies.GetAlimonyByFamily;

public record struct GetAlimonyByFamilyQuery(Guid CourtId, Guid FamilyId) : IQuery<AlimonyResponse>;