using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.VisitationLocations.DeleteVisitationLocation;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitationLocations;

internal sealed class DeleteVisitationLocation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.VisitationLocations.Delete, async (
            Guid locationId,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new DeleteVisitationLocationCommand(user.GetCourtId(), locationId));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.VisitationLocations)
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(DeleteVisitationLocation))
        .RequireAuthorization(CustomPolicies.CourtStaffOnly);
    }
}