using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Custodies.GetCustodyByFamily;
using Wesal.Contracts.Custodies;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Custodies;

internal sealed class GetCustodyByFamily : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Custodies.GetByFamily, async (Guid familyId, ISender sender) =>
        {
            var result = await sender.Send(new GetCustodyByFamilyQuery(familyId));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Custodies)
        .Produces<CustodyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(GetCustodyByFamily));
    }
}