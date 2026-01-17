using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Data;
using Wesal.Application.Schools.ListSchools;
using Wesal.Contracts.Common;
using Wesal.Contracts.Schools;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Schools;

internal sealed class ListSchools : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Schools.List, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ISender sender) =>
        {
            var result = await sender.Send(new ListSchoolsQuery(SharedData.StaffId, query.Name, pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Schools)
        .Produces<PagedResponse<SchoolResponse>>(StatusCodes.Status200OK)
        .WithOpenApiName(nameof(ListSchools));
    }

    internal record struct QueryParams(string? Name = null);
}