using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.Families.ReturnToDispute;

internal sealed class ReturnToDisputeCommandHandler(
    IFamilyRepository familyRepository,
    ICourtStaffRepository staffRepository) : ICommandHandler<ReturnToDisputeCommand>
{
    public async Task<Result> Handle(
        ReturnToDisputeCommand request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        if (family.AssignedStaffId != request.SettlementStaffId)
            return Error.Forbidden("Family.Ownership", "You are not assigned to this family.");

        if (family.Status != FamilyStatus.Settled)
            return Error.Validation("Family.NotSettled", "Only settled families can return to dispute.");

        family.Reactivate();
        familyRepository.Update(family);

        var settlementStaff = await staffRepository.GetByIdWithWorkloadAsync(request.SettlementStaffId, cancellationToken);
        settlementStaff!.IncrementLoad(AssignmentType.Family);

        return Result.Success;
    }
}