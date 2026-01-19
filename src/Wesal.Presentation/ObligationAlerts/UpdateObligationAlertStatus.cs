using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.ObligationAlerts.UpdateObligationAlertStatus;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.ObligationAlerts;

internal sealed class UpdateObligationAlertStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(ApiEndpoints.ObligationAlerts.UpdateStatus, async (
            Guid alertId,
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateObligationAlertStatusCommand(
                alertId,
                SharedData.StaffUserId,
                request.Status,
                request.ResolutionNotes));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.ObligationAlerts)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(UpdateObligationAlertStatus));
    }

    internal readonly record struct Request(string Status, string ResolutionNotes);
}