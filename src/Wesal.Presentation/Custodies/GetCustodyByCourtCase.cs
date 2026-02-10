using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Custodies.GetCustodyByCourtCase;
using Wesal.Contracts.Custodies;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Custodies;

internal sealed class GetCustodyByCourtCase : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Custodies.GetByCourtCase, async (Guid courtCaseId, ISender sender) =>
        {
            var result = await sender.Send(new GetCustodyByCourtCaseQuery(courtCaseId));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Custodies)
        .Produces<CustodyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(GetCustodyByCourtCase))
        .RequireAuthorization();
    }
}