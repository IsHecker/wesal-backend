using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Data;
using Wesal.Application.ObligationAlerts.ListObligationAlerts;
using Wesal.Contracts.ObligationAlerts;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.ObligationAlerts;

internal sealed class ListObligationAlerts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.ObligationAlerts.ListByStaff, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListObligationAlertsQuery(
                user.GetRoleId(),
                query.Status,
                query.ViolationType,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.ObligationAlerts)
        .Produces<ObligationAlertsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(ListObligationAlerts))
        .RequireAuthorization(CustomPolicies.ComplianceMonitorOnly);
    }

    internal record struct QueryParams(string? Status = null, string? ViolationType = null);
}