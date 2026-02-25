using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitationSchedules.CreateVisitationSchedule;

internal sealed class CreateVisitationScheduleCommandHandler(
    ICourtCaseRepository courtCaseRepository,
    ICustodyRepository custodyRepository,
    IParentRepository parentRepository,
    IVisitationLocationRepository visitLocationRepository,
    IVisitationScheduleRepository visitationScheduleRepository)
    : ICommandHandler<CreateVisitationScheduleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateVisitationScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var courtCase = await courtCaseRepository.GetByIdAsync(request.CourtCaseId, cancellationToken);
        if (courtCase is null)
            return CourtCaseErrors.NotFound(request.CourtCaseId);

        var validationResult = await ValidateSchedule(request, courtCase, cancellationToken);
        if (validationResult.IsFailure)
            return validationResult.Error;

        var custody = await custodyRepository.GetByFamilyIdAsync(courtCase.FamilyId, cancellationToken);
        if (custody is null)
            return VisitationScheduleErrors.CustodyNotFound;

        var custodialParent = await parentRepository.GetByIdAsync(custody!.CustodialParentId, cancellationToken);
        var nonCustodialParent = await parentRepository.GetByIdAsync(custody.NonCustodialParentId, cancellationToken);

        if (!Enum.TryParse<VisitationFrequency>(request.Frequency, out var frequency))
            return VisitationScheduleErrors.InvalidFrequency(request.Frequency);

        var schedule = VisitationSchedule.Create(
            courtCase.CourtId,
            request.CourtCaseId,
            courtCase.FamilyId,
            custodialParent!,
            nonCustodialParent!,
            request.LocationId,
            frequency,
            request.StartDate,
            request.EndDate,
            request.StartTime,
            request.EndTime);

        await visitationScheduleRepository.AddAsync(schedule, cancellationToken);

        return schedule.Id;
    }

    private async Task<Result> ValidateSchedule(
        CreateVisitationScheduleCommand request,
        CourtCase courtCase,
        CancellationToken cancellationToken)
    {
        if (courtCase.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        var isScheduleExist = await visitationScheduleRepository.ExistsByCourtCaseIdAsync(courtCase.Id, cancellationToken);
        if (isScheduleExist)
            return VisitationScheduleErrors.AlreadyExistForCase;

        var isLocationExist = await visitLocationRepository.ExistsAsync(request.LocationId, cancellationToken);
        if (!isLocationExist)
            return VisitationLocationErrors.NotFound(request.LocationId);

        return Result.Success;
    }
}