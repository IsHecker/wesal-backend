using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.VisitationSchedules.DeleteVisitationSchedule;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitationSchedules;

internal sealed class DeleteVisitationSchedule : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.VisitationSchedules.Delete, async (
            Guid scheduleId,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new DeleteVisitationScheduleCommand(
                user.GetCourtId(),
                scheduleId));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.VisitationSchedules)
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(DeleteVisitationSchedule))
        .RequireAuthorization(CustomPolicies.CourtStaffOnly);
    }
}