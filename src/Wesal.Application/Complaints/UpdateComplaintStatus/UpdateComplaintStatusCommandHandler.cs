using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Complaints;
using Wesal.Domain.Results;

namespace Wesal.Application.Complaints.UpdateComplaintStatus;

internal sealed class UpdateComplaintStatusCommandHandler(
    IComplaintRepository complaintRepository,
    ICourtStaffRepository courtStaffRepository)
    : ICommandHandler<UpdateComplaintStatusCommand>
{
    public async Task<Result> Handle(
        UpdateComplaintStatusCommand request,
        CancellationToken cancellationToken)
    {
        var complaint = await complaintRepository.GetByIdAsync(request.ComplaintId, cancellationToken);
        if (complaint is null)
            return ComplaintErrors.NotFound(request.ComplaintId);

        if (complaint.CourtId != request.CourtId)
            return ComplaintErrors.ComplaintMismatch;

        if (complaint.AssignedStaffId != request.StaffId)
            return Error.Forbidden("Complaint.Ownership", "You are not assigned to this complaint.");

        var updateResult = complaint.UpdateStatus(request.Status.ToEnum<ComplaintStatus>(), request.ResolutionNotes);
        if (updateResult.IsFailure)
            return updateResult;

        if (complaint.Status is ComplaintStatus.Resolved or ComplaintStatus.Rejected)
        {
            var staff = await courtStaffRepository.GetByIdWithWorkloadAsync(complaint.AssignedStaffId, cancellationToken);
            staff!.DecrementLoad(AssignmentType.Complaint);
        }

        complaintRepository.Update(complaint);
        return Result.Success;
    }
}