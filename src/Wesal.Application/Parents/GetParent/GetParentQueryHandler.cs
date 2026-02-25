using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.Parents;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.Parents.GetParent;

internal sealed class GetParentQueryHandler(
    IParentRepository parentRepository)
    : IQueryHandler<GetParentQuery, ParentResponse>
{
    public async Task<Result<ParentResponse>> Handle(
        GetParentQuery request,
        CancellationToken cancellationToken)
    {
        var parent = await parentRepository.GetByIdAsync(request.ParentId, cancellationToken);
        if (parent is null)
            return ParentErrors.NotFound(request.ParentId);

        if (parent!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(Family));

        return parent.ToResponse();
    }
}