using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.VisitationSchedules;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.VisitationSchedules.ListVisitationSchedulesByFamily;

internal sealed class ListVisitationSchedulesByFamilyQueryHandler(
    IFamilyRepository familyRepository,
    IWesalDbContext context)
    : IQueryHandler<ListVisitationSchedulesByFamilyQuery, PagedResponse<VisitationScheduleResponse>>
{
    public async Task<Result<PagedResponse<VisitationScheduleResponse>>> Handle(
        ListVisitationSchedulesByFamilyQuery request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByIdAsync(request.FamilyId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFound(request.FamilyId);

        var query = context.VisitationSchedules
            .Where(s => s.FamilyId == request.FamilyId)
            .OrderBy(s => s.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .Paginate(request.Pagination)
            .Select(schedule => new VisitationScheduleResponse(
                schedule.Id,
                schedule.CourtCaseId,
                schedule.FamilyId,
                schedule.ParentId,
                schedule.LocationId,
                schedule.StartDayInMonth,
                schedule.Frequency.ToString(),
                schedule.StartTime,
                schedule.EndTime))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}