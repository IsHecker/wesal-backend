using Wesal.Contracts.Children;
using Wesal.Contracts.Parents;

namespace Wesal.Contracts.Families;

public record struct FamilyResponse(
    Guid FamilyId,
    ParentProfileResponse Father,
    ParentProfileResponse Mother,
    IEnumerable<ChildResponse> Children);