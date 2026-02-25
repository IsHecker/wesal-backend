using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.CustodyRequests;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.CustodyRequests.ListCustodyRequests;

internal sealed class ListCustodyRequestsQueryHandler(
    IFamilyRepository familyRepository,
    IParentRepository parentRepository,
    IWesalDbContext context)
    : IQueryHandler<ListCustodyRequestsQuery, CustodyRequestsResponse>
{
    public async Task<Result<CustodyRequestsResponse>> Handle(
        ListCustodyRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var queryResult = await BuildAuthorizedQuery(request, cancellationToken);
        if (queryResult.IsFailure)
            return queryResult.Error;

        var pendingCount = await queryResult.Value.CountAsync(
            request => request.Status == CustodyRequestStatus.Pending,
            cancellationToken);

        var approvedCount = await queryResult.Value.CountAsync(
            request => request.Status == CustodyRequestStatus.Approved,
            cancellationToken);

        var rejectedCount = await queryResult.Value.CountAsync(
            request => request.Status == CustodyRequestStatus.Rejected,
            cancellationToken);

        var query = ApplyStatusFilter(queryResult.Value, request.Status);

        var totalCount = await query.CountAsync(cancellationToken);

        var pagedRequests = await query
            .OrderByDescending(cr => cr.CreatedAt)
            .Paginate(request.Pagination)
            .Select(request => new CustodyRequestResponse(
                request.Id,
                request.FamilyId,
                request.CourtCaseId,
                request.CustodyId,
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

        return new CustodyRequestsResponse(
            pagedRequests,
            pendingCount,
            approvedCount,
            rejectedCount);
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
        var baseQuery = context.CustodyRequests
            .Include(cr => cr.Family)
                .ThenInclude(f => f.Father)
            .Include(cr => cr.Family)
                .ThenInclude(f => f.Mother);

        return request.UserRole == UserRole.Parent
            ? await BuildParentQuery(baseQuery, request.UserId, request.FamilyId!.Value, cancellationToken)
            : baseQuery.Where(cr => cr.Family.CourtId == request.CourtId).ToResult();
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