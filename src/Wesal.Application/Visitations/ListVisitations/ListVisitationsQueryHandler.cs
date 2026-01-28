using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Visitations;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
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
        var validationResult = await ValidateFilters(request, cancellationToken);
        if (validationResult.IsFailure)
            return validationResult.Error;

        var query = BuildQuery(request, validationResult.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .Paginate(request.Pagination)
            .Select(visitation => new VisitationResponse(
                visitation.Id,
                visitation.FamilyId,
                visitation.ParentId,
                visitation.LocationId,
                visitation.VisitationScheduleId,
                visitation.StartAt,
                visitation.EndAt,
                visitation.CompletedAt,
                visitation.Status.ToString(),
                visitation.CheckedInAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }

    private async Task<Result<Guid?>> ValidateFilters(
        ListVisitationsQuery request,
        CancellationToken cancellationToken)
    {
        if (request.FamilyId.HasValue)
        {
            var familyExists = await familyRepository.ExistsAsync(request.FamilyId.Value, cancellationToken);
            if (!familyExists)
                return FamilyErrors.NotFound(request.FamilyId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.NationalId))
        {
            var parent = await context.Parents
                .FirstOrDefaultAsync(parent => parent.NationalId == request.NationalId, cancellationToken);

            if (parent is null)
                return ParentErrors.NotFoundByNationalId(request.NationalId);

            return parent.Id;
        }

        return Guid.Empty;
    }

    private IQueryable<Visitation> BuildQuery(ListVisitationsQuery request, Guid? parentId)
    {
        var query = context.Visitations.AsQueryable();

        if (request.FamilyId.HasValue)
            query = query.Where(v => v.FamilyId == request.FamilyId.Value);

        if (parentId.HasValue && parentId != Guid.Empty)
            query = query.Where(v => v.ParentId == parentId.Value);

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var status = request.Status.ToEnum<VisitationStatus>();
            query = query.Where(v => v.Status == status);
        }

        if (request.Date.HasValue)
            query = query.Where(v => v.StartAt.ToDateOnly() == request.Date.Value);

        return query.OrderByDescending(v => v.StartAt.ToDateOnly());
    }
}