using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Children.AddChild;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

internal sealed class AddChildCommandHandler(
    IFamilyRepository familyRepository,
    IChildRepository childRepository)
    : ICommandHandler<AddChildCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        AddChildCommand request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        if (family!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(Family));

        var child = Child.Create(
            request.FamilyId,
            request.FullName,
            request.BirthDate,
            request.Gender,
            request.SchoolId);

        await childRepository.AddAsync(child, cancellationToken);

        return child.Id;
    }
}