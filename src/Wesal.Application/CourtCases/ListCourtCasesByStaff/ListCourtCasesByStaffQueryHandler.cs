using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.CourtCases;
using Wesal.Domain.Results;

namespace Wesal.Application.CourtCases.ListCourtCasesByStaff;

internal sealed class ListCourtCasesByStaffQueryHandler(
    IWesalDbContext context)
    : IQueryHandler<ListCourtCasesByStaffQuery, PagedResponse<CourtCaseResponse>>
{
    public async Task<Result<PagedResponse<CourtCaseResponse>>> Handle(
        ListCourtCasesByStaffQuery request,
        CancellationToken cancellationToken)
    {
        var courtCases = context.CourtCases
            .Where(c => c.AssignedStaffId == request.StaffId);

        if (!string.IsNullOrWhiteSpace(request.CaseNumber))
        {
            var searchTerm = request.CaseNumber.Trim().ToLower();
            courtCases = courtCases.Where(c => c.CaseNumber.ToLower().Contains(searchTerm));
        }

        var totalCount = await courtCases.CountAsync(cancellationToken);

        return await courtCases
            .OrderByDescending(c => c.FiledAt)
            .Paginate(request.Pagination)
            .Select(courtCase => courtCase.ToResponse())
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}