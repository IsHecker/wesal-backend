using Wesal.Application.Messaging;
using Wesal.Contracts.Families;

namespace Wesal.Application.Families.GetFamily;

public record struct GetFamilyQuery(Guid CourtId, Guid FamilyId) : IQuery<FamilyResponse>;