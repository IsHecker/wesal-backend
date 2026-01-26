using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Complaints;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Results;

namespace Wesal.Application.Complaints.CreateComplaint;

internal sealed class CreateComplaintCommandHandler(
    IParentRepository parentRepository,
    IComplaintRepository complaintRepository,
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

        var count = await complaintRepository.GetMonthCountByParentIdAsync(request.ParentId, cancellationToken);

        if (count >= complaintOptions.MaxComplaintsInMonth)
            return ComplaintErrors.MaxCountExceeded(complaintOptions.MaxComplaintsInMonth);

        var complaint = Complaint.Create(
            parent.CourtId,
            request.ParentId,
            request.Type.ToEnum<ComplaintType>(),
            request.Description);

        await complaintRepository.AddAsync(complaint, cancellationToken);

        return complaint.Id;
    }
}