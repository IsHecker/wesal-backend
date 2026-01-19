using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.VisitationSchedules.CreateVisitationSchedule;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitationSchedules;

internal sealed class CreateVisitationSchedule : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.VisitationSchedules.Create, async (
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new CreateVisitationScheduleCommand(
                SharedData.StaffUserId,
                request.CourtCaseId,
                request.ParentId,
                request.LocationId,
                request.StartDayInMonth,
                request.Frequency,
                request.StartTime,
                request.EndTime));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.VisitationSchedules)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(CreateVisitationSchedule));
    }

    internal sealed record Request(
        Guid CourtCaseId,
        Guid ParentId,
        Guid LocationId,
        int StartDayInMonth,
        string Frequency,
        TimeOnly StartTime,
        TimeOnly EndTime);
}