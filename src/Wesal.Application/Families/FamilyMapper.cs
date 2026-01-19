using Wesal.Application.Parents;
using Wesal.Contracts.Children;
using Wesal.Contracts.Families;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Families;

namespace Wesal.Application.Families;

public static class FamilyMapper
{
    public static FamilyResponse ToResponse(this Family family) =>
        new(
            family.Id,
            family.Father.ToResponse(),
            family.Mother.ToResponse(),
            family.Children.Select(child => child.ToResponse()));

    private static ChildResponse ToResponse(this Child child) =>
        new(
            child.Id,
            child.FullName,
            child.SchoolId,
            child.Gender,
            child.BirthDate,
            AgeCalculator.CalculateAge(child.BirthDate));
}