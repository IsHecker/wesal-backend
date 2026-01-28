using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.VisitationSchedules.CreateVisitationSchedule;
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
                request.CourtCaseId,
                request.ParentId,
                request.LocationId,
                request.Frequency,
                request.StartDate,
                request.EndDate,
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
        string Frequency,
        DateOnly StartDate,
        DateOnly EndDate,
        TimeOnly StartTime,
        TimeOnly EndTime);
}