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
        app.MapGet(ApiEndpoints.ObligationAlerts.List, async (
            [AsParameters] QueryParams query,
            ClaimsPrincipal user,
            [AsParameters] Pagination pagination,
            ISender sender) =>
        {
            var result = await sender.Send(new ListObligationAlertsQuery(
                user.GetCourtId(),
                query.Status,
                query.ViolationType,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.ObligationAlerts)
        .Produces<ObligationAlertsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(ListObligationAlerts))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }

    internal record struct QueryParams(string? Status = null, string? ViolationType = null);
}