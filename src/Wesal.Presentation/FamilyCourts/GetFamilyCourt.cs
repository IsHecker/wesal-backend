using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.FamilyCourts.GetFamilyCourt;
using Wesal.Contracts.FamilyCourts;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.FamilyCourts;

internal sealed class GetFamilyCourt : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Courts.GetById, async (Guid courtId, ISender sender) =>
        {
            var result = await sender.Send(new GetFamilyCourtQuery(courtId));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Courts)
        .Produces<FamilyCourtResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(GetFamilyCourt))
        .RequireAuthorization();
    }
}