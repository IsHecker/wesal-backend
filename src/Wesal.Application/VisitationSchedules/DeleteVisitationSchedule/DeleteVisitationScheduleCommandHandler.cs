using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Application.VisitationSchedules.DeleteVisitationSchedule;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Domain.Results;

internal sealed class DeleteVisitationScheduleCommandHandler(
    IRepository<VisitationSchedule> scheduleRepository,
    ICourtCaseRepository courtCaseRepository)
    : ICommandHandler<DeleteVisitationScheduleCommand>
{
    public async Task<Result> Handle(
        DeleteVisitationScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var schedule = await scheduleRepository
            .GetByIdAsync(request.ScheduleId, cancellationToken);

        if (schedule is null)
            return VisitationScheduleErrors.NotFound(request.ScheduleId);

        var courtCase = await courtCaseRepository
            .GetByIdAsync(schedule.CourtCaseId, cancellationToken);

        if (courtCase!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        if (courtCase!.Status == CourtCaseStatus.Closed)
            return VisitationScheduleErrors.CannotModifyClosedCase;

        // TODO: Hard block: completed or checked-in visitations are legal evidence ---
        // var hasCompleted = await visitationRepository
        //     .HasCompletedByScheduleIdAsync(request.ScheduleId, cancellationToken);
        // if (hasCompleted)
        //     return VisitationScheduleErrors.HasCompletedVisitations;

        // TODO: Safe to delete: cancel all remaining pending visitations first ---
        // await visitationRepository
        //     .CancelPendingByScheduleIdAsync(request.ScheduleId, cancellationToken);

        scheduleRepository.Delete(schedule);

        return Result.Success;
    }
}