using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Data;
using Wesal.Application.VisitCenterStaffs.ListVisitCenterStaffsByCourt;
using Wesal.Contracts.Common;
using Wesal.Contracts.VisitCenterStaffs;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitCenterStaffs;

internal sealed class ListVisitCenterStaffs : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.VisitCenterStaffs.ListByCourt, async (
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListVisitCenterStaffsByCourtQuery(
                user.GetCourtId(),
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.VisitCenterStaffs)
        .Produces<PagedResponse<VisitCenterStaffResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(ListVisitCenterStaffs))
        .RequireAuthorization(CustomPolicies.CourtAdminOnly);
    }
}