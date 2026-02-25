using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.CustodyRequests.ListCustodyRequests;
using Wesal.Application.Data;
using Wesal.Contracts.CustodyRequests;
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
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListCustodyRequestsQuery(
                user.GetRoleId(),
                user.GetCourtId(),
                user.GetRole(),
                query.FamilyId,
                query.Status,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CustodyRequests)
        .Produces<CustodyRequestsResponse>(StatusCodes.Status200OK)
        .WithOpenApiName(nameof(ListCustodyRequests))
        .RequireAuthorization(new AuthorizeAttribute
        {
            Roles = $"{UserRole.FamilyCourt},{UserRole.CourtStaff},{UserRole.Parent}"
        });
    }

    internal record struct QueryParams(Guid? FamilyId = null, string? Status = null);
}