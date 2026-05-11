using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Data;
using Wesal.Application.FamilyCourts.ListFamilyCourts;
using Wesal.Contracts.Common;
using Wesal.Contracts.FamilyCourts;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.FamilyCourts;

internal sealed class ListFamilyCourts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Courts.List, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ISender sender) =>
        {
            var result = await sender.Send(new ListFamilyCourtsQuery(query.Name, query.Governorate, pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Courts)
        .Produces<PagedResponse<FamilyCourtResponse>>(StatusCodes.Status200OK)
        .WithOpenApiName(nameof(ListFamilyCourts))
        .RequireAuthorization(CustomPolicies.SystemAdminOnly);
    }

    internal record struct QueryParams(string? Name = null, string? Governorate = null);
}