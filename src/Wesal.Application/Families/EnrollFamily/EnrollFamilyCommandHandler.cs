using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Families;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.EnrollFamily;

internal sealed class EnrollFamilyCommandHandler(
    IParentRepository parentRepository,
    IFamilyRepository familyRepository,
    IChildRepository childRepository) : ICommandHandler<EnrollFamilyCommand, EnrollFamilyResponse>
{
    public async Task<Result<EnrollFamilyResponse>> Handle(
        EnrollFamilyCommand request,
        CancellationToken cancellationToken)
    {
        // TODO: Father can be in many families.
        if (await parentRepository.ExistsByNationalIdAsync(request.Father.NationalId, cancellationToken))
            return FamilyErrors.ParentAlreadyExists(request.Father.NationalId);

        if (await parentRepository.ExistsByNationalIdAsync(request.Mother.NationalId, cancellationToken))
            return FamilyErrors.ParentAlreadyExists(request.Mother.NationalId);

        var father = Parent.Create(
            request.Father.NationalId,
            request.Father.FullName,
            request.Father.BirthDate,
            request.Father.Gender,
            request.Father.Job,
            request.Father.Address,
            request.Father.Phone,
            request.Father.Email);

        var mother = Parent.Create(
            request.Mother.NationalId,
            request.Mother.FullName,
            request.Mother.BirthDate,
            request.Mother.Gender,
            request.Mother.Job,
            request.Mother.Address,
            request.Mother.Phone,
            request.Mother.Email);

        // TODO: Create User accounts with for both parents temporary passwords

        await parentRepository.AddAsync(father, cancellationToken);
        await parentRepository.AddAsync(mother, cancellationToken);

        var family = Family.Create(father.Id, mother.Id);

        await familyRepository.AddAsync(family, cancellationToken);

        foreach (var childDto in request.Children ?? [])
        {
            var child = Child.Create(
                family.Id,
                childDto.FullName,
                childDto.BirthDate,
                childDto.Gender,
                childDto.SchoolId);

            await childRepository.AddAsync(child, cancellationToken);
        }

        return new EnrollFamilyResponse(family.Id, "", "");
    }
}