using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.CustodyRequests.ListCustodyRequests;
using Wesal.Application.Data;
using Wesal.Contracts.Common;
using Wesal.Contracts.CustodyRequests;
using Wesal.Domain;
using Wesal.Domain.Entities.Users;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.CustodyRequests;

internal sealed class ListCustodyRequests : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.CustodyRequests.ListByCourt, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ISender sender) =>
        {
            var result = await sender.Send(new ListCustodyRequestsQuery(
                SharedData.StaffId,
                UserRole.CourtStaff, // TODO: Extract user role from token.
                query.Status,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CustodyRequests)
        .Produces<PagedResponse<CustodyRequestResponse>>(StatusCodes.Status200OK)
        .WithOpenApiName(nameof(ListCustodyRequests));
    }

    internal record struct QueryParams(string? Status = null);
}