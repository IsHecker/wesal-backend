using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Parents.GetParent;
using Wesal.Contracts.Parents;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Parents;

internal sealed class GetParent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Parents.GetById, async (
            Guid parentId,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new GetParentQuery(
                user.GetCourtId(),
                parentId));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Parents)
        .Produces<ParentResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(GetParent))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }
}