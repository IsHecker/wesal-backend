using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.CustodyRequests;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.CustodyRequests.ListCustodyRequests;

internal sealed class ListCustodyRequestsQueryHandler(
    ICourtStaffRepository courtStaffRepository,
    IParentRepository parentRepository,
    IWesalDbContext context)
    : IQueryHandler<ListCustodyRequestsQuery, PagedResponse<CustodyRequestResponse>>
{
    public async Task<Result<PagedResponse<CustodyRequestResponse>>> Handle(
        ListCustodyRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = await BuildQuery(request, cancellationToken);
        if (baseQuery.IsFailure)
            return baseQuery.Error;

        var query = ApplyFilters(baseQuery.Value, request.Status);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
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
                .ThenInclude(f => f.Mother)
            .AsQueryable();

        if (request.UserRole == UserRole.CourtStaff)
        {
            var staff = await courtStaffRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (staff is null)
                return CourtStaffErrors.NotFound(request.UserId);

            return baseQuery.Where(x => x.Family.CourtId == staff.CourtId)
                .ToResult();
        }

        var parent = await parentRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (parent is null)
            return ParentErrors.NotFound(request.UserId);

        return baseQuery.Where(cr => cr.ParentId == request.UserId).ToResult();
    }

    private IQueryable<CustodyRequest> ApplyFilters(IQueryable<CustodyRequest> query, string? status)
    {
        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(cr => cr.Status == status.ToEnum<CustodyRequestStatus>());

        return query.OrderByDescending(cr => cr.CreatedAt);
    }
}