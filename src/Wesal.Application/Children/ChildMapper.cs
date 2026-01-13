using Wesal.Contracts.Children;
using Wesal.Domain.Common;
using Wesal.Domain.Entities.Children;

namespace Wesal.Application.Children;

public static class ChildMapper
{
    public static ChildResponse ToResponse(this Child child) =>
        new(
            child.Id,
            child.FullName,
            child.SchoolId,
            child.Gender,
            child.BirthDate,
            AgeCalculator.CalculateAge(child.BirthDate));
}