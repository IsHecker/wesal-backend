using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Data;
using Wesal.Application.VisitationLocations.ListVisitationLocations;
using Wesal.Contracts.Common;
using Wesal.Contracts.VisitationLocations;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitationLocations;

internal sealed class ListVisitationLocations : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.VisitationLocations.List, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ISender sender) =>
        {
            var result = await sender.Send(new ListVisitationLocationsQuery(
                SharedData.StaffUserId,
                query.Name,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.VisitationLocations)
        .Produces<PagedResponse<VisitationLocationResponse>>(StatusCodes.Status200OK)
        .WithOpenApiName(nameof(ListVisitationLocations));
    }

    internal sealed record QueryParams(string? Name = null);
}