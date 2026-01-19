using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Extensions;
using Wesal.Application.Messaging;
using Wesal.Contracts.Common;
using Wesal.Contracts.SchoolReports;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.SchoolReports;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.SchoolReports.ListSchoolReportsByChild;

internal sealed class ListSchoolReportsByChildQueryHandler(
    ISchoolRepository schoolRepository,
    IChildRepository childRepository,
    IWesalDbContext context)
    : IQueryHandler<ListSchoolReportsByChildQuery, PagedResponse<SchoolReportResponse>>
{
    public async Task<Result<PagedResponse<SchoolReportResponse>>> Handle(
        ListSchoolReportsByChildQuery request,
        CancellationToken cancellationToken)
    {
        var school = await schoolRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (school is null)
            return UserErrors.NotFound(request.UserId);

        var child = await childRepository.GetByIdAsync(request.ChildId, cancellationToken);
        if (child is null)
            return ChildErrors.NotFound(request.ChildId);

        if (child.SchoolId != school.Id)
            return SchoolReportErrors.ChildNotInSchool();

        var query = context.SchoolReports
            .OrderByDescending(report => report.UploadedAt)
            .Where(report => report.ChildId == request.ChildId)
            .OrderByDescending(report => report.UploadedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        return await query
            .Paginate(request.Pagination)
            .Select(report => new SchoolReportResponse(
                report.Id,
                report.ChildId,
                report.ReportType.ToString(),
                report.UploadedAt))
            .ToPagedResponseAsync(request.Pagination, totalCount);
    }
}