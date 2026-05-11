using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.CourtCases.ListCourtCasesByStaff;
using Wesal.Application.Data;
using Wesal.Contracts.Common;
using Wesal.Contracts.CourtCases;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.CourtCases;

internal sealed class ListCourtCasesByStaff : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.CourtCases.ListByStaff, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListCourtCasesByStaffQuery(
                user.GetRoleId(),
                query.CaseNumber,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CourtCases)
        .Produces<PagedResponse<CourtCaseResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListCourtCasesByStaff))
        .RequireAuthorization(CustomPolicies.CaseClerkOnly);
    }

    internal record struct QueryParams(string? CaseNumber = null);
}