using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.VisitationSchedules.GetVisitationScheduleByCourtCase;
using Wesal.Contracts.VisitationSchedules;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitationSchedules;

internal sealed class GetVisitationScheduleByCourtCase : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.VisitationSchedules.GetByCourtCase, async (
            Guid courtCaseId,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new GetVisitationScheduleByCourtCaseQuery(
                user.GetCourtId(),
                courtCaseId));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.VisitationSchedules)
        .Produces<VisitationScheduleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(GetVisitationScheduleByCourtCase))
        .RequireAuthorization(CustomPolicies.CourtAndParents);
    }
}