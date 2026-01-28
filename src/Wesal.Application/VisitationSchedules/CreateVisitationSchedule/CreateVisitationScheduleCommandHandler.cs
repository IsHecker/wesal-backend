using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitationSchedules.CreateVisitationSchedule;

internal sealed class CreateVisitationScheduleCommandHandler(
    ICourtCaseRepository courtCaseRepository,
    IFamilyRepository familyRepository,
    IParentRepository parentRepository,
    IRepository<VisitationLocation> visitLocationRepository,
    IRepository<VisitationSchedule> visitationScheduleRepository)
    : ICommandHandler<CreateVisitationScheduleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateVisitationScheduleCommand request,
        CancellationToken cancellationToken)
    {
        var courtCase = await courtCaseRepository.GetByIdAsync(request.CourtCaseId, cancellationToken);
        if (courtCase is null)
            return CourtCaseErrors.NotFound(request.CourtCaseId);

        var validationResult = await ValidateSchedule(request, courtCase.FamilyId, cancellationToken);
        if (validationResult.IsFailure)
            return validationResult.Error;

        if (!Enum.TryParse<VisitationFrequency>(request.Frequency, out var frequency))
            return VisitationScheduleErrors.InvalidFrequency(request.Frequency);

        var schedule = VisitationSchedule.Create(
            courtCase.CourtId,
            request.CourtCaseId,
            courtCase.FamilyId,
            request.ParentId,
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
        Guid familyId,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(familyId, cancellationToken)
            ?? throw new InvalidOperationException();

        var isParentExist = await parentRepository.ExistsAsync(request.ParentId, cancellationToken);
        if (!isParentExist)
            return ParentErrors.NotFound(request.ParentId);

        if (family.FatherId != request.ParentId && family.MotherId != request.ParentId)
            return VisitationScheduleErrors.ParentNotInFamily();

        var isLocationExist = await visitLocationRepository.ExistsAsync(request.LocationId, cancellationToken);
        if (!isLocationExist)
            return VisitationLocationErrors.NotFound(request.LocationId);

        return Result.Success;
    }
}