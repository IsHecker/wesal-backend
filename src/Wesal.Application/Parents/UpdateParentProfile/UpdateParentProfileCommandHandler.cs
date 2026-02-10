using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.Parents.UpdateParentProfile;

internal sealed class UpdateParentProfileCommandHandler(
    IParentRepository parentRepository)
    : ICommandHandler<UpdateParentProfileCommand>
{
    public async Task<Result> Handle(
        UpdateParentProfileCommand request,
        CancellationToken cancellationToken)
    {
        var parent = await parentRepository.GetByIdAsync(request.ParentId, cancellationToken);
        if (parent is null)
            return ParentErrors.NotFound(request.ParentId);

        if (parent!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(Family));

        parent.UpdateProfile(request.Email, request.Job, request.Address, request.Phone);

        parentRepository.Update(parent);

        return Result.Success;
    }
}