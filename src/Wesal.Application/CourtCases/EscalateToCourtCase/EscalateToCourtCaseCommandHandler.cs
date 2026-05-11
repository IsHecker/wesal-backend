using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Documents;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtCases.EscalateToCourtCase;

internal sealed class EscalateToCourtCaseCommandHandler(
    ICourtCaseRepository courtCaseRepository,
    ICourtStaffRepository staffRepository,
    IFamilyRepository familyRepository,
    IRepository<Document> documentRepository,
    IAutoAssignmentService autoAssignmentService)
    : ICommandHandler<EscalateToCourtCaseCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        EscalateToCourtCaseCommand request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        if (family.AssignedStaffId != request.SettlementStaffId)
            return Error.Forbidden("Family.Ownership", "You are not assigned to this family.");

        var hasOpenCase = await courtCaseRepository.HasOpenCaseByFamilyIdAsync(
            request.FamilyId,
            cancellationToken);

        if (hasOpenCase)
            return CourtCaseErrors.FamilyHasOpenCase;

        if (await courtCaseRepository.ExistsByCaseNumberAsync(request.CaseNumber, cancellationToken))
            return CourtCaseErrors.CaseNumberAlreadyExists(request.CaseNumber);

        if (request.DocumentId is not null)
        {
            var isExist = await documentRepository.ExistsAsync(request.DocumentId.Value, cancellationToken);
            if (!isExist)
                return DocumentErrors.NotFound(request.DocumentId.Value);
        }

        var assignedClerk = await autoAssignmentService.GetLowestLoadStaffAsync(
            request.CourtId,
            StaffRole.CaseClerk,
            AssignmentType.CourtCase,
            cancellationToken);

        var courtCase = CourtCase.Create(
            assignedClerk.CourtId,
            request.FamilyId,
            request.CaseNumber,
            request.DecisionSummary,
            assignedClerk.Id,
            request.DocumentId);

        assignedClerk.IncrementLoad(AssignmentType.CourtCase);
        // staffRepository.Update(assignedClerk);

        var oldStatus = family.Status;
        family.Resolve(FamilyStatus.Escalated);
        familyRepository.Update(family);

        var settlementStaff = await staffRepository.GetByIdWithWorkloadAsync(request.SettlementStaffId, cancellationToken);
        
        if (oldStatus == FamilyStatus.Active)
        {
            settlementStaff!.DecrementLoad(AssignmentType.Family);
        }

        // staffRepository.Update(settlementStaff);
        await courtCaseRepository.AddAsync(courtCase, cancellationToken);
        return courtCase.Id;
    }
}