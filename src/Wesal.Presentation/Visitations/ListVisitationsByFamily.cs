using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Data;
using Wesal.Application.Visitations.ListVisitationsByFamily;
using Wesal.Contracts.Common;
using Wesal.Contracts.Visitations;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Visitations;

internal sealed class ListVisitationsByFamily : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Visitations.ListByFamily, async (
            Guid familyId,
            [AsParameters] Pagination pagination,
            ISender sender) =>
        {
            var result = await sender.Send(new ListVisitationsByFamilyQuery(familyId, pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Visitations)
        .Produces<PagedResponse<VisitationResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListVisitationsByFamily));
    }
}