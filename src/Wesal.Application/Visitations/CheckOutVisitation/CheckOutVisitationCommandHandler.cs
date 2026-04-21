using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitCenterStaffs;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.CheckOutVisitation;

internal sealed class CheckOutVisitationCommandHandler(
    IVisitationRepository visitationRepository,
    IVisitCenterStaffRepository centerStaffRepository,
    IOptions<VisitationOptions> options) : ICommandHandler<CheckOutVisitationCommand>
{
    private readonly VisitationOptions options = options.Value;

    public async Task<Result> Handle(CheckOutVisitationCommand request, CancellationToken cancellationToken)
    {
        var visitation = await visitationRepository.GetByIdAsync(request.VisitationId, cancellationToken);

        if (visitation is null)
            return VisitationErrors.NotFound(request.VisitationId);

        var staff = await centerStaffRepository.GetByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return VisitCenterStaffErrors.NotFound(request.StaffId);

        visitationRepository.Update(visitation);

        return visitation.CheckOut(staff, request.NationalId, options.CheckOutGracePeriodMinutes, options.CheckInGracePeriodMinutes);
    }
}