using Wesal.Application.Messaging;
using Wesal.Contracts.Parents;

namespace Wesal.Application.Parents.GetParent;

public record struct GetParentQuery(
    Guid CourtId,
    Guid ParentId) : IQuery<ParentResponse>;