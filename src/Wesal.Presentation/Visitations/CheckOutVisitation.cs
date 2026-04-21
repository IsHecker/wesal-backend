using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Visitations.CheckOutVisitation;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Visitations;

internal sealed class CheckOutVisitation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(ApiEndpoints.Visitations.CheckOut, async (
            Guid visitationId,
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new CheckOutVisitationCommand(
                user.GetRoleId(),
                request.NationalId,
                visitationId));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Visitations)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(CheckOutVisitation))
        .RequireAuthorization(CustomPolicies.VisitCenterStaffOnly);
    }

    internal record struct Request(string NationalId);
}