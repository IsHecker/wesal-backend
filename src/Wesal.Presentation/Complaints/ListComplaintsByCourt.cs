using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Complaints.ListComplaintsByCourt;
using Wesal.Application.Data;
using Wesal.Contracts.Common;
using Wesal.Contracts.Complaints;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Complaints;

internal sealed class ListComplaintsByCourt : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Complaints.ListByCourt, async (
            [AsParameters] Pagination pagination,
            ISender sender) =>
        {
            var result = await sender.Send(new ListComplaintsByCourtQuery(
                SharedData.StaffId,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Complaints)
        .Produces<PagedResponse<ComplaintResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListComplaintsByCourt));
    }
}