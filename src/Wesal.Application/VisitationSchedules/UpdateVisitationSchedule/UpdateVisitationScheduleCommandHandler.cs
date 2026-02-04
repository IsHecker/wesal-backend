using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Application.VisitationSchedules.UpdateVisitationSchedule;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Domain.Results;

internal sealed class UpdateVisitationScheduleCommandHandler(
    IRepository<VisitationSchedule> visitationScheduleRepository,
    IVisitationLocationRepository visitationLocationRepository,
    ICourtCaseRepository courtCaseRepository)
    : ICommandHandler<UpdateVisitationScheduleCommand>
{
    public async Task<Result> Handle(
        UpdateVisitationScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var schedule = await visitationScheduleRepository
            .GetByIdAsync(request.ScheduleId, cancellationToken);
        if (schedule is null)
            return VisitationScheduleErrors.NotFound(request.ScheduleId);

        var courtCase = await courtCaseRepository
            .GetByIdAsync(schedule.CourtCaseId, cancellationToken);

        if (courtCase!.Status == CourtCaseStatus.Closed)
            return VisitationScheduleErrors.CannotModifyClosedCase;

        if (courtCase!.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        // TODO: Block if any visitation from this schedule is already completed (legal evidence) ---
        // var hasCompleted = await visitationRepository
        //     .HasCompletedByScheduleIdAsync(request.ScheduleId, cancellationToken);
        // if (hasCompleted)
        //     return VisitationScheduleErrors.HasCompletedVisitations;

        // TODO: Cancel all future pending visitations generated from this schedule.
        //     The background job / caller is responsible for triggering regeneration. ---
        // await visitationRepository
        //     .CancelPendingByScheduleIdAsync(request.ScheduleId, cancellationToken);

        var isLocationExist = await visitationLocationRepository.ExistsAsync(
            request.LocationId,
            cancellationToken);

        if (!isLocationExist)
            return VisitationLocationErrors.NotFound(request.LocationId);

        schedule.Update(
            request.LocationId,
            request.Frequency.ToEnum<VisitationFrequency>(),
            request.StartTime,
            request.EndTime,
            request.StartDate,
            request.EndDate);

        return Result.Success;
    }
}