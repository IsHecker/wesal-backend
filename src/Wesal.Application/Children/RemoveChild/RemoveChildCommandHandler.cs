using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Children.RemoveChild;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

internal sealed class RemoveChildCommandHandler(
    IFamilyRepository familyRepository,
    IChildRepository childRepository)
    : ICommandHandler<RemoveChildCommand>
{
    public async Task<Result> Handle(
        RemoveChildCommand request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        if (family!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(Family));

        var child = await childRepository.GetByIdAsync(request.ChildId, cancellationToken);
        if (child is null)
            return ChildErrors.NotFound(request.ChildId);

        if (child.FamilyId != request.FamilyId)
            return ChildErrors.NotBelongsToFamily;

        // TODO: Block if child has an active school link ---
        // var hasSchoolLink = await schoolRepository
        //     .ExistsByChildIdAsync(request.ChildId, cancellationToken);

        // TODO: Block if child is referenced in any custody decision ---
        // var hasCustodyLink = await custodyRepository
        //     .ExistsByChildIdAsync(request.ChildId, cancellationToken);

        // if (hasSchoolLink || hasCustodyLink)
        //     return ChildErrors.HasSchoolOrCustodyLinks;

        childRepository.Delete(child);

        return Result.Success;
    }
}