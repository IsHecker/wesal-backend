using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Notifications.MarkNotificationAsRead;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Notifications;

internal sealed class MarkNotificationAsRead : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(ApiEndpoints.Notifications.MarkAsRead, async (
            Guid notificationId,
            ISender sender) =>
        {
            var result = await sender.Send(new MarkNotificationAsReadCommand(
                notificationId,
                SharedData.FatherId));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Notifications)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(MarkNotificationAsRead));
    }
}