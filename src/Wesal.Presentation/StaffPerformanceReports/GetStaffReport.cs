using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.StaffPerformanceReports.GetStaffReport;
using Wesal.Contracts.StaffPerformanceReports;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.StaffPerformanceReports;

internal sealed class GetStaffReport : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // Admin viewing someone else's report
        app.MapGet(ApiEndpoints.CourtStaffs.GetReport, async (
            Guid staffId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetStaffReportQuery(staffId));
            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CourtStaffs)
        .Produces<StaffPerformanceReportResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization(CustomPolicies.CourtAdminOnly);

        // Staff viewing their own report
        app.MapGet(ApiEndpoints.CourtStaffs.GetMyReport, async (
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new GetStaffReportQuery(user.GetRoleId()));
            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CourtStaffs)
        .Produces<StaffPerformanceReportResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization(CustomPolicies.CourtStaffOnly);
    }
}