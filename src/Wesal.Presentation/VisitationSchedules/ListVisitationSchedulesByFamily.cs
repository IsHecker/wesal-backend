using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Data;
using Wesal.Application.VisitationSchedules.ListVisitationSchedulesByFamily;
using Wesal.Contracts.Common;
using Wesal.Contracts.VisitationSchedules;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitationSchedules;

internal sealed class ListVisitationSchedulesByFamily : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.VisitationSchedules.ListByFamily, async (
            Guid familyId,
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListVisitationSchedulesByFamilyQuery(
                user.GetRoleId(),
                familyId,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.VisitationSchedules)
        .Produces<PagedResponse<VisitationScheduleResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListVisitationSchedulesByFamily))
        .RequireAuthorization(CustomPolicies.CourtStaffOnly);
    }
}