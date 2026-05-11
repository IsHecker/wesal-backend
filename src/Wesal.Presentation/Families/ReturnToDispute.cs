using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Families.ReturnToDispute;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Families;

internal sealed class ReturnToDispute : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Families.ReturnToDispute, async (
            Guid familyId,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ReturnToDisputeCommand(
                familyId,
                user.GetRoleId()));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Families)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ReturnToDispute))
        .RequireAuthorization(CustomPolicies.SettlementSpecialistOnly);
    }
}