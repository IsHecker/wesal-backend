using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Families.ListFamiliesByParent;
using Wesal.Contracts.Families;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Families;

internal sealed class ListFamiliesByParent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Families.GetByParent, async (ClaimsPrincipal user, ISender sender) =>
        {
            var result = await sender.Send(new ListFamiliesByParentQuery(user.GetRoleId()));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Families)
        .Produces<IEnumerable<FamilyResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .WithOpenApiName(nameof(ListFamiliesByParent))
        .RequireAuthorization(CustomPolicies.ParentsOnly);
    }
}