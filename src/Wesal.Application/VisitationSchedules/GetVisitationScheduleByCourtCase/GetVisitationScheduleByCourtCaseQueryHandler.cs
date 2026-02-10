using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Contracts.VisitationSchedules;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.VisitationSchedules;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitationSchedules.GetVisitationScheduleByCourtCase;

internal sealed class GetVisitationScheduleByCourtCaseQueryHandler(
    ICourtCaseRepository courtCaseRepository,
    IWesalDbContext dbContext)
    : IQueryHandler<GetVisitationScheduleByCourtCaseQuery, VisitationScheduleResponse>
{
    public async Task<Result<VisitationScheduleResponse>> Handle(
        GetVisitationScheduleByCourtCaseQuery request,
        CancellationToken cancellationToken)
    {
        var courtCase = await courtCaseRepository.GetByIdAsync(request.CourtCaseId, cancellationToken);
        if (courtCase is null)
            return CourtCaseErrors.NotFound(request.CourtCaseId);

        if (courtCase.CourtId != request.CourtId)
            return FamilyCourtErrors.NotBelongToCourt(nameof(CourtCase));

        var schedule = await dbContext.VisitationSchedules
            .FirstOrDefaultAsync(s => s.CourtCaseId == request.CourtCaseId, cancellationToken);

        if (schedule is null)
            return VisitationScheduleErrors.NotFoundByCourtCase(request.CourtCaseId);

        return new VisitationScheduleResponse(
            schedule.Id,
            schedule.CourtCaseId,
            schedule.FamilyId,
            schedule.NonCustodialParentId,
            schedule.LocationId,
            schedule.Frequency.ToString(),
            schedule.StartDate,
            schedule.EndDate,
            schedule.StartTime,
            schedule.EndTime);
    }
}