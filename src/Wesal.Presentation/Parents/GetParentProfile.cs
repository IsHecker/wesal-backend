using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Parents.GetParentProfile;
using Wesal.Contracts.Parents;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Parents;

internal sealed class GetParentProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Parents.Profile, async (ClaimsPrincipal user, ISender sender) =>
        {
            var result = await sender.Send(new GetParentProfileQuery(user.GetRoleId()));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Parents)
        .Produces<ParentProfileResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(GetParentProfile))
        .RequireAuthorization(CustomPolicies.ParentsOnly);
    }
}