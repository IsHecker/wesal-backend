using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Data;
using Wesal.Application.Schools.ListSchools;
using Wesal.Contracts.Common;
using Wesal.Contracts.Schools;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Schools;

internal sealed class ListSchools : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Schools.List, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListSchoolsQuery(user.GetCourtId(), query.Name, pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Schools)
        .Produces<PagedResponse<SchoolResponse>>(StatusCodes.Status200OK)
        .WithOpenApiName(nameof(ListSchools))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }

    internal record struct QueryParams(string? Name = null);
}