using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Custodies.DeleteCustody;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Custodies;

internal sealed class DeleteCustody : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Custodies.Delete, async (
            Guid custodyId,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new DeleteCustodyCommand(user.GetCourtId(), custodyId));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Custodies)
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(DeleteCustody))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }
}