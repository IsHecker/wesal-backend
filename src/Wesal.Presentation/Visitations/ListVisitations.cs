using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Data;
using Wesal.Application.Visitations.ListVisitations;
using Wesal.Contracts.Common;
using Wesal.Contracts.Visitations;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Visitations;

internal sealed class ListVisitations : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Visitations.List, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ISender sender) =>
        {
            var result = await sender.Send(new ListVisitationsQuery(
                query.FamilyId,
                query.NationalId,
                query.Status,
                query.Date,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Visitations)
        .Produces<PagedResponse<VisitationResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListVisitations))
        .RequireAuthorization();
    }

    internal record struct QueryParams(
        Guid? FamilyId = null,
        string? NationalId = null,
        string? Status = null,
        DateOnly? Date = null);
}