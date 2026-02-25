using Wesal.Contracts.Parents;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Application.Parents;

public static class ParentMapper
{
    public static ParentResponse ToResponse(this Parent parent) =>
        new(
            parent.Id,
            parent.FullName,
            parent.NationalId,
            parent.BirthDate,
            parent.Gender,
            parent.Job,
            parent.Address,
            parent.Phone,
            parent.Email
        );
}