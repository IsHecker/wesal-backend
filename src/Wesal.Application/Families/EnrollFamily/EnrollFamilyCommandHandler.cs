using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.Families;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.EnrollFamily;

internal sealed class EnrollFamilyCommandHandler(
    IRepository<User> userRepository,
    ICourtStaffRepository staffRepository,
    IParentRepository parentRepository,
    IFamilyRepository familyRepository,
    IChildRepository childRepository) : ICommandHandler<EnrollFamilyCommand, EnrollFamilyResponse>
{
    public async Task<Result<EnrollFamilyResponse>> Handle(
        EnrollFamilyCommand request,
        CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (staff is null)
            return UserErrors.NotFound(request.UserId);

        // TODO: Father can be in many families.
        if (await parentRepository.ExistsByNationalIdAsync(request.Father.NationalId, cancellationToken))
            return FamilyErrors.ParentAlreadyExists(request.Father.NationalId);

        if (await parentRepository.ExistsByNationalIdAsync(request.Mother.NationalId, cancellationToken))
            return FamilyErrors.ParentAlreadyExists(request.Mother.NationalId);

        var fatherUser = User.Create(UserRole.Parent, request.Father.NationalId, "");
        var motherUser = User.Create(UserRole.Parent, request.Father.NationalId, "");

        var father = Parent.Create(
            fatherUser.Id,
            staff.CourtId,
            request.Father.NationalId,
            request.Father.FullName,
            request.Father.BirthDate,
            request.Father.Gender,
            request.Father.Job,
            request.Father.Address,
            request.Father.Phone,
            request.Father.Email);

        var mother = Parent.Create(
            motherUser.Id,
            staff.CourtId,
            request.Mother.NationalId,
            request.Mother.FullName,
            request.Mother.BirthDate,
            request.Mother.Gender,
            request.Mother.Job,
            request.Mother.Address,
            request.Mother.Phone,
            request.Mother.Email);

        // TODO: Create User accounts with for both parents temporary passwords

        await parentRepository.AddRangeAsync([father, mother], cancellationToken);

        await userRepository.AddRangeAsync([fatherUser, motherUser], cancellationToken);

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