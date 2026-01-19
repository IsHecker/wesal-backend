using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Parents;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Parents.GetParentProfile;

internal sealed class GetParentProfileCommandHandler(IParentRepository parentRepository)
    : IQueryHandler<GetParentProfileQuery, ParentProfileResponse>
{
    public async Task<Result<ParentProfileResponse>> Handle(
        GetParentProfileQuery request,
        CancellationToken cancellationToken)
    {
        var parent = await parentRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (parent is null)
            return UserErrors.NotFound(request.UserId);

        return parent.ToResponse();
    }
}