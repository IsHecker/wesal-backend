using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.SettleFamily;

internal sealed class SettleFamilyCommandHandler(
    IFamilyRepository familyRepository,
    ICourtStaffRepository staffRepository) : ICommandHandler<SettleFamilyCommand>
{
    public async Task<Result> Handle(
        SettleFamilyCommand request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        if (family.AssignedStaffId != request.SettlementStaffId)
            return Error.Forbidden("Family.Ownership", "You are not assigned to this family.");

        if (family.Status != FamilyStatus.Active)
            return Error.Validation("Family.NotActive", "Family is not currently active.");

        family.Resolve(FamilyStatus.Settled);
        familyRepository.Update(family);

        var settlementStaff = await staffRepository.GetByIdWithWorkloadAsync(request.SettlementStaffId, cancellationToken);
        settlementStaff!.DecrementLoad(AssignmentType.Family);
        // staffRepository.Update(settlementStaff);

        return Result.Success;
    }
}