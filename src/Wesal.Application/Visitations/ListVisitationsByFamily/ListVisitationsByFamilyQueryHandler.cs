using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Visitations;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.ListVisitationsByFamily;

internal sealed class ListVisitationsByFamilyQueryHandler(
    IFamilyRepository familyRepository,
    IWesalDbContext context)
    : IQueryHandler<ListVisitationsByFamilyQuery, PagedResponse<VisitationResponse>>
{
    public async Task<Result<PagedResponse<VisitationResponse>>> Handle(
        ListVisitationsByFamilyQuery request,
        CancellationToken cancellationToken)
    {
        var isFamilyExist = await familyRepository.ExistsAsync(request.FamilyId, cancellationToken);

        if (!isFamilyExist)
            return FamilyErrors.NotFound(request.FamilyId);

        var scheduledVisitations = context.Visitations
            .Where(vistation => vistation.FamilyId == request.FamilyId);

        _ = scheduledVisitations.TryGetNonEnumeratedCount(out var totalCount);

        return await scheduledVisitations
            .Paginate(request.Pagination)
            .Select(visitation => new VisitationResponse(
                visitation.Id,
                visitation.FamilyId,
                visitation.ParentId,
                visitation.LocationId,
                visitation.VisitationScheduleId,
                visitation.ScheduledVisitAt,
                visitation.VisitedAt,
                visitation.Status.ToString(),
                visitation.IsCheckedIn,
                visitation.CheckedInAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}