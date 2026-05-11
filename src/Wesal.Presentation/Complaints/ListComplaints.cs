using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Complaints.ListComplaints;
using Wesal.Application.Data;
using Wesal.Contracts.Complaints;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Complaints;

internal sealed class ListComplaints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Complaints.ListByStaff, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListComplaintsQuery(
                user.GetRoleId(),
                query.Status,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Complaints)
        .Produces<ComplaintsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListComplaints))
        .RequireAuthorization(CustomPolicies.ComplianceMonitorOnly);
    }

    internal record struct QueryParams(string? Status = null);
}