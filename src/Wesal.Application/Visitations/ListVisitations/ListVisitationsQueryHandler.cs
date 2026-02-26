using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Visitations;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Results;

namespace Wesal.Application.Visitations.ListVisitations;

internal sealed class ListVisitationsQueryHandler(
    IFamilyRepository familyRepository,
    IWesalDbContext context)
    : IQueryHandler<ListVisitationsQuery, PagedResponse<VisitationResponse>>
{
    public async Task<Result<PagedResponse<VisitationResponse>>> Handle(
        ListVisitationsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.FamilyId.HasValue)
        {
            var familyExists = await familyRepository.ExistsAsync(request.FamilyId.Value, cancellationToken);
            if (!familyExists)
                return FamilyErrors.NotFound(request.FamilyId.Value);
        }

        var query = BuildQuery(request, request.NationalId);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .Paginate(request.Pagination)
            .Select(visitation => new VisitationResponse(
                visitation.Id,
                visitation.FamilyId,
                visitation.NonCustodialParentId,
                visitation.NonCustodialNationalId,
                visitation.CompanionNationalId,
                visitation.LocationId,
                visitation.VisitationScheduleId,
                visitation.StartAt,
                visitation.EndAt,
                visitation.Status.ToString(),
                visitation.NonCustodialCheckedInAt,
                visitation.CompanionCheckedInAt,
                visitation.CompletedAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }

    private IQueryable<Visitation> BuildQuery(ListVisitationsQuery request, string? nationalId)
    {
        var query = context.Visitations.AsQueryable();

        if (request.FamilyId.HasValue)
            query = query.Where(v => v.FamilyId == request.FamilyId.Value);

        if (!string.IsNullOrWhiteSpace(nationalId))
            query = query.Where(v => nationalId == v.CompanionNationalId
                || nationalId == v.NonCustodialNationalId);

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var status = request.Status.ToEnum<VisitationStatus>();
            query = query.Where(v => v.Status == status);
        }

        if (request.Date.HasValue)
        {
            var startAt = new DateTime(
                request.Date.Value.Year,
                request.Date.Value.Month,
                request.Date.Value.Day);

            query = query.Where(v => v.StartAt.Date == startAt);
        }

        return query.OrderBy(v => v.StartAt);
    }
}