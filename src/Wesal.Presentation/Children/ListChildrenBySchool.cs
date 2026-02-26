using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Children.ListChildrenBySchool;
using Wesal.Application.Data;
using Wesal.Contracts.Children;
using Wesal.Contracts.Common;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Children;

internal sealed class ListChildrenBySchool : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Children.ListBySchool, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListChildrenBySchoolQuery(
                user.GetRoleId(),
                query.Name,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Children)
        .Produces<PagedResponse<ChildResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListChildrenBySchool))
        .RequireAuthorization(CustomPolicies.SchoolsOnly);
    }

    internal record struct QueryParams(string? Name = null);
}