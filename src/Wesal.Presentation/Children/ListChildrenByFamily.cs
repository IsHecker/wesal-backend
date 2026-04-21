using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Children.ListChildrenByFamily;
using Wesal.Contracts.Children;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Children;

internal sealed class ListChildrenByFamily : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Families.Children, async (Guid familyId, ISender sender) =>
        {
            var result = await sender.Send(new ListChildrenByFamilyQuery(familyId));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Children)
        .Produces<IEnumerable<ChildResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListChildrenByFamily))
        .RequireAuthorization();
    }
}