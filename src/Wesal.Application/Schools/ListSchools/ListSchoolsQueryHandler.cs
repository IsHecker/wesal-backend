using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.Schools;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Results;

namespace Wesal.Application.Schools.ListSchools;

internal sealed class ListSchoolsQueryHandler(IWesalDbContext context)
    : IQueryHandler<ListSchoolsQuery, PagedResponse<SchoolResponse>>
{
    public async Task<Result<PagedResponse<SchoolResponse>>> Handle(
        ListSchoolsQuery request,
        CancellationToken cancellationToken)
    {
        var staff = await GetStaffByIdAsync(request.StaffId, cancellationToken);
        if (staff is null)
            return CourtStaffErrors.NotFound(request.StaffId);

        var query = BuildQuery(request.Name, staff.Court.Governorate);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .Paginate(request.Pagination)
            .Select(school => new SchoolResponse(
                school.Id,
                school.Name,
                school.Address,
                school.Governorate,
                school.Email,
                school.ContactNumber))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }

    private IQueryable<School> BuildQuery(string? name, string governorate)
    {
        var query = context.Schools
            .AsQueryable()
            .Where(s => s.Governorate == governorate);

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(s => s.Name.Contains(name));

        return query.OrderBy(s => s.Name);
    }

    private Task<CourtStaff?> GetStaffByIdAsync(Guid staffId, CancellationToken cancellationToken)
    {
        return context.CourtStaffs
            .Include(staff => staff.Court)
            .FirstOrDefaultAsync(staff => staff.Id == staffId, cancellationToken);
    }
}