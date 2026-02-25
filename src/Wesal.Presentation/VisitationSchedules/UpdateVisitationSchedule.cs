using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.VisitationSchedules.UpdateVisitationSchedule;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitationSchedules;

internal sealed class UpdateVisitationSchedule : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.VisitationSchedules.Update, async (
            Guid scheduleId,
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateVisitationScheduleCommand(
                user.GetCourtId(),
                scheduleId,
                request.LocationId,
                request.Frequency,
                request.StartTime,
                request.EndTime,
                request.StartDate,
                request.EndDate));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.VisitationSchedules)
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(UpdateVisitationSchedule))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }

    internal record struct Request(
        Guid ScheduleId,
        string Frequency,
        TimeOnly StartTime,
        TimeOnly EndTime,
        Guid LocationId,
        DateOnly StartDate,
        DateOnly? EndDate);
}