using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.CourtStaffs.ListCourtStaffsByCourt;
using Wesal.Application.Data;
using Wesal.Contracts.Common;
using Wesal.Contracts.CourtStaffs;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.CourtStaffs;

internal sealed class ListCourtStaffs : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.CourtStaffs.ListByCourt, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListCourtStaffsByCourtQuery(
                user.GetCourtId(),
                query.Role,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CourtStaffs)
        .Produces<PagedResponse<CourtStaffResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(ListCourtStaffs))
        .RequireAuthorization(CustomPolicies.CourtAdminOnly);
    }

    internal record struct QueryParams(string? Role = null);
}