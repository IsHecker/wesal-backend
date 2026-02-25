using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Custodies.UpdateCustody;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Custodies;

internal sealed class UpdateCustody : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Custodies.Update, async (
            Guid custodyId,
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateCustodyCommand(
                user.GetCourtId(),
                custodyId,
                request.NewCustodialParentId,
                request.StartAt,
                request.EndAt));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Custodies)
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(UpdateCustody))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }

    internal record struct Request(
        Guid NewCustodialParentId,
        DateTime StartAt,
        DateTime? EndAt);
}