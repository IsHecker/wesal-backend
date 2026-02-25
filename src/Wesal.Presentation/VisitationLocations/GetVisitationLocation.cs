using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.VisitationLocations.GetVisitationLocation;
using Wesal.Contracts.VisitationLocations;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitationLocations;

internal sealed class GetVisitationLocation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.VisitationLocations.GetById, async (
            Guid locationId,
            ISender sender) =>
        {
            var result = await sender.Send(new GetVisitationLocationQuery(locationId));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.VisitationLocations)
        .Produces<VisitationLocationResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(GetVisitationLocation))
        .RequireAuthorization();
    }
}