using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Complaints;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Documents;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.Complaints.CreateComplaint;

internal sealed class CreateComplaintCommandHandler(
IParentRepository parentRepository,
IFamilyRepository familyRepository,
IComplaintRepository complaintRepository,
IRepository<Document> documentRepository,
ICourtStaffRepository courtStaffRepository,
IAutoAssignmentService autoAssignmentService,
IOptions<ComplaintOptions> complaintOptions)
: ICommandHandler<CreateComplaintCommand, Guid>
{
    private readonly ComplaintOptions complaintOptions = complaintOptions.Value;

    public async Task<Result<Guid>> Handle(
        CreateComplaintCommand request,
        CancellationToken cancellationToken)
    {
        var parent = await parentRepository.GetByIdAsync(request.ParentId, cancellationToken);
        if (parent is null)
            return ParentErrors.NotFound(request.ParentId);

        var isInFamily = await familyRepository.IsParentInFamilyAsync(request.ParentId, request.FamilyId, cancellationToken);
        if (!isInFamily)
            return FamilyErrors.ParentNotInFamily;

        // var count = await complaintRepository.GetMonthCountByParentIdAsync(request.ParentId, cancellationToken);

        // if (count >= complaintOptions.MaxComplaintsInMonth)
        //     return ComplaintErrors.MaxCountExceeded(complaintOptions.MaxComplaintsInMonth);

        if (request.DocumentId is not null)
        {
            var isExist = await documentRepository.ExistsAsync(request.DocumentId.Value, cancellationToken);
            if (!isExist)
                return DocumentErrors.NotFound(request.DocumentId.Value);
        }

        var assignedMonitor = await autoAssignmentService.GetBalancedComplianceMonitorAsync(
            parent.CourtId,
            AssignmentType.Complaint,
            cancellationToken);

        var complaint = Complaint.Create(
            parent.CourtId,
            request.FamilyId,
            request.ParentId,
            request.Type.ToEnum<ComplaintType>(),
            request.Description,
            assignedMonitor.Id,
            request.DocumentId);

        assignedMonitor.IncrementLoad(AssignmentType.Complaint);
        // courtStaffRepository.Update(assignedMonitor);

        await complaintRepository.AddAsync(complaint, cancellationToken);

        return complaint.Id;
    }
}