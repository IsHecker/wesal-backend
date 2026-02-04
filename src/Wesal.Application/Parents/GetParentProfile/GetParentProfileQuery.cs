using Wesal.Application.Messaging;
using Wesal.Contracts.Parents;

namespace Wesal.Application.Parents.GetParentProfile;

public record struct GetParentProfileQuery(Guid ParentId) : IQuery<ParentProfileResponse>;