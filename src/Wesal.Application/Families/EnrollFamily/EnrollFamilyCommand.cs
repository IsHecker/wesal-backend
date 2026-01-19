using Wesal.Application.Families.EnrollFamily.Dtos;
using Wesal.Application.Messaging;
using Wesal.Contracts.Families;

namespace Wesal.Application.Families.EnrollFamily;

public record struct EnrollFamilyCommand(
    Guid UserId,
    CreateParentDto Father,
    CreateParentDto Mother,
    IEnumerable<CreateChildDto>? Children = null) : ICommand<EnrollFamilyResponse>;