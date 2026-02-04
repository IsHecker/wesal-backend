using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.CustodyRequests;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.CustodyRequests.ListCustodyRequests;

internal sealed class ListCustodyRequestsQueryHandler(
    ICourtStaffRepository courtStaffRepository,
    IFamilyRepository familyRepository,
    IParentRepository parentRepository,
    IWesalDbContext context)
    : IQueryHandler<ListCustodyRequestsQuery, PagedResponse<CustodyRequestResponse>>
{
    public async Task<Result<PagedResponse<CustodyRequestResponse>>> Handle(
        ListCustodyRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var queryResult = await BuildAuthorizedQuery(request, cancellationToken);
        if (queryResult.IsFailure)
            return queryResult.Error;

        var query = ApplyStatusFilter(queryResult.Value, request.Status);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .OrderByDescending(cr => cr.CreatedAt)
            .Paginate(request.Pagination)
            .Select(request => new CustodyRequestResponse(
                request.Id,
                request.Family.Father.FullName,
                request.Family.Mother.FullName,
                request.StartDate,
                request.EndDate,
                request.Reason,
                request.Status.ToString(),
                request.CreatedAt,
                request.DecisionNote,
                request.ProcessedAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }

    private async Task<Result<IQueryable<CustodyRequest>>> BuildQuery(
        ListCustodyRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = context.CustodyRequests
            .Include(cr => cr.Family)
                .ThenInclude(f => f.Father)
            .Include(cr => cr.Family)
                .ThenInclude(f => f.Mother);

        if (request.UserRole == UserRole.CourtStaff)
        {
            var staff = await courtStaffRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (staff is null)
                return CourtStaffErrors.NotFound(request.UserId);

            return baseQuery.Where(x => x.Family.CourtId == staff.CourtId).ToResult();
        }

        var parent = await parentRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (parent is null)
            return ParentErrors.NotFound(request.UserId);

        return baseQuery.Where(cr => cr.FamilyId == request.UserId).ToResult();
    }

    private static IQueryable<CustodyRequest> ApplyStatusFilter(
        IQueryable<CustodyRequest> query,
        string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return query;

        var statusEnum = status.ToEnum<CustodyRequestStatus>();
        return query.Where(cr => cr.Status == statusEnum);
    }

    private async Task<Result<IQueryable<CustodyRequest>>> BuildAuthorizedQuery(
        ListCustodyRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = GetBaseQuery();

        return request.UserRole == UserRole.CourtStaff
            ? await BuildCourtStaffQuery(baseQuery, request.UserId, cancellationToken)
            : await BuildParentQuery(baseQuery, request.UserId, request.FamilyId!.Value, cancellationToken);
    }

    private IQueryable<CustodyRequest> GetBaseQuery()
    {
        return context.CustodyRequests
            .Include(cr => cr.Family)
                .ThenInclude(f => f.Father)
            .Include(cr => cr.Family)
                .ThenInclude(f => f.Mother);
    }

    private async Task<Result<IQueryable<CustodyRequest>>> BuildCourtStaffQuery(
        IQueryable<CustodyRequest> baseQuery,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var staff = await courtStaffRepository.GetByIdAsync(userId, cancellationToken);
        if (staff is null)
            return CourtStaffErrors.NotFound(userId);

        return baseQuery.Where(cr => cr.Family.CourtId == staff.CourtId).ToResult();
    }

    private async Task<Result<IQueryable<CustodyRequest>>> BuildParentQuery(
        IQueryable<CustodyRequest> baseQuery,
        Guid userId,
        Guid familyId,
        CancellationToken cancellationToken)
    {
        var parentResult = await GetParentAsync(userId, familyId, cancellationToken);
        if (parentResult.IsFailure)
            return parentResult.Error;

        return baseQuery.Where(cr => cr.FamilyId == familyId).ToResult();
    }

    private async Task<Result<Parent>> GetParentAsync(
        Guid userId,
        Guid familyId,
        CancellationToken cancellationToken)
    {
        var parent = await parentRepository.GetByIdAsync(userId, cancellationToken);
        if (parent is null)
            return ParentErrors.NotFound(userId);

        var isInFamily = await familyRepository.IsParentInFamilyAsync(parent.Id, familyId, cancellationToken);
        if (!isInFamily)
            return FamilyErrors.ParentNotInFamily;

        return parent;
    }
}