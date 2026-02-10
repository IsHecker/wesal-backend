using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Visitations.SetCompanionForVisitation;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Visitations;

internal sealed class SetCompanionForVisitation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(ApiEndpoints.Visitations.GetById, async (
            Guid visitationId,
            Request requst,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new SetCompanionForVisitationCommand(
                user.GetRoleId(),
                visitationId,
                requst.CompanionNationalId));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Visitations)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(SetCompanionForVisitation))
        .RequireAuthorization(CustomPolicies.ParentsOnly);
    }

    internal record struct Request(string CompanionNationalId);
}