using Wesal.Application.Children;
using Wesal.Application.Parents;
using Wesal.Contracts.Families;
using Wesal.Domain.Entities.Families;

namespace Wesal.Application.Families;

public static class FamilyMapper
{
    public static FamilyResponse ToResponse(this Family family) =>
        new(
            family.Id,
            family.Father.ToResponse(),
            family.Mother.ToResponse(),
            family.Children.Select(c => c.ToResponse()));
}