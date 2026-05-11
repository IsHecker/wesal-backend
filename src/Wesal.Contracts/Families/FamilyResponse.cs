using Wesal.Contracts.Children;
using Wesal.Contracts.Parents;

namespace Wesal.Contracts.Families;

public record struct FamilyResponse(
    Guid FamilyId,
    string Status,
    ParentResponse Father,
    ParentResponse Mother,
    IEnumerable<ChildResponse> Children);