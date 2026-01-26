using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Complaints;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.Complaints.UpdateComplaintStatus;

internal sealed class UpdateComplaintStatusCommandHandler(
    ICourtStaffRepository staffRepository,
    IComplaintRepository complaintRepository)
    : ICommandHandler<UpdateComplaintStatusCommand>
{
    public async Task<Result> Handle(
        UpdateComplaintStatusCommand request,
        CancellationToken cancellationToken)
    {
        var staff = await staffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return CourtStaffErrors.NotFound(request.StaffId);

        var complaint = await complaintRepository.GetByIdAsync(request.ComplaintId, cancellationToken);
        if (complaint is null)
            return ComplaintErrors.NotFound(request.ComplaintId);

        if (complaint.CourtId != staff.CourtId)
            return ComplaintErrors.ComplaintMismatch;

        complaintRepository.Update(complaint);

        return complaint.UpdateStatus(request.Status.ToEnum<ComplaintStatus>(), request.ResolutionNotes);
    }
}