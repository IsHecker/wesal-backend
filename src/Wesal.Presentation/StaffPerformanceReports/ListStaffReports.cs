using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Data;
using Wesal.Application.StaffPerformanceReports.ListStaffReports;
using Wesal.Contracts.Common;
using Wesal.Contracts.StaffPerformanceReports;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.StaffPerformanceReports;

internal sealed class ListStaffReports : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.CourtStaffs.ListReports, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListStaffReportsQuery(user.GetCourtId(), query.Role, pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CourtStaffs)
        .Produces<PagedResponse<StaffPerformanceSummaryResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .WithOpenApiName(nameof(ListStaffReports))
        .RequireAuthorization(CustomPolicies.CourtAdminOnly);
    }

    internal record struct QueryParams(string? Role = null);
}