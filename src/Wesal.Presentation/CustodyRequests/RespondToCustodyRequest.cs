using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.CustodyRequests.RespondToCustodyRequest;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.CustodyRequests;

internal sealed class RespondToCustodyRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(ApiEndpoints.CustodyRequests.Respond, async (
            Guid requestId,
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new RespondToCustodyRequestCommand(
                requestId,
                user.GetRoleId(),
                request.IsAccepted,
                request.ReasonNote));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.CustodyRequests)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(RespondToCustodyRequest))
        .RequireAuthorization(CustomPolicies.ParentsOnly);
    }

    internal record struct Request(
        bool IsAccepted,
        string? ReasonNote);
}