using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Parents;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.Parents.GetParentProfile;

internal sealed class GetParentProfileQueryHandler(IParentRepository parentRepository)
    : IQueryHandler<GetParentProfileQuery, ParentProfileResponse>
{
    public async Task<Result<ParentProfileResponse>> Handle(
        GetParentProfileQuery request,
        CancellationToken cancellationToken)
    {
        var parent = await parentRepository.GetByIdAsync(request.ParentId, cancellationToken);

        if (parent is null)
            return ParentErrors.NotFound(request.ParentId);

        return parent.ToResponse();
    }
}