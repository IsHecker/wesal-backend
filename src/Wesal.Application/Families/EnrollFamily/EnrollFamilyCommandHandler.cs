using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Messaging;
using Wesal.Contracts.Families;
using Wesal.Contracts.Users;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.EnrollFamily;

internal sealed class EnrollFamilyCommandHandler(
    ICourtStaffRepository staffRepository,
    IParentRepository parentRepository,
    IFamilyRepository familyRepository,
    IChildRepository childRepository,
    IUserService userService) : ICommandHandler<EnrollFamilyCommand, EnrollFamilyResponse>
{
    public async Task<Result<EnrollFamilyResponse>> Handle(
        EnrollFamilyCommand request,
        CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return UserErrors.NotFound(request.StaffId);

        // TODO: Father can be in many families.
        if (await parentRepository.ExistsByNationalIdAsync(request.Father.NationalId, cancellationToken))
            return FamilyErrors.ParentAlreadyExists(request.Father.NationalId);

        if (await parentRepository.ExistsByNationalIdAsync(request.Mother.NationalId, cancellationToken))
            return FamilyErrors.ParentAlreadyExists(request.Mother.NationalId);

        var fatherUser = await userService.CreateAsync(UserRole.Parent, cancellationToken);
        var motherUser = await userService.CreateAsync(UserRole.Parent, cancellationToken);

        var father = Parent.Create(
            fatherUser.User.Id,
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
            motherUser.User.Id,
            staff.CourtId,
            request.Mother.NationalId,
            request.Mother.FullName,
            request.Mother.BirthDate,
            request.Mother.Gender,
            request.Mother.Job,
            request.Mother.Address,
            request.Mother.Phone,
            request.Mother.Email);

        var family = Family.Create(staff.CourtId, father.Id, mother.Id);

        await parentRepository.AddRangeAsync([father, mother], cancellationToken);
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

        return new EnrollFamilyResponse(
            family.Id,
            new UserCredentialResponse(father.Id, father.NationalId, fatherUser.TemporaryPassword),
            new UserCredentialResponse(mother.Id, mother.NationalId, motherUser.TemporaryPassword));
    }
}