using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.VisitationLocations.UpdateVisitationLocation;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitationLocations;

internal sealed class UpdateVisitationLocation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.VisitationLocations.Update, async (
            Guid locationId,
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateVisitationLocationCommand(
                locationId,
                SharedData.StaffUserId,
                request.Name,
                request.Address,
                request.Governorate,
                request.ContactNumber,
                request.MaxConcurrentVisits,
                request.OpeningTime,
                request.ClosingTime));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.VisitationLocations)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(UpdateVisitationLocation));
    }

    internal record struct Request(
        string Name,
        string Address,
        string Governorate,
        string? ContactNumber,
        int MaxConcurrentVisits,
        TimeOnly OpeningTime,
        TimeOnly ClosingTime
    );
}