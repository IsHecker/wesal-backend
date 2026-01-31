using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Data;
using Wesal.Application.Notifications.ListNotificationsByUserQuery;
using Wesal.Contracts.Notifications;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Notifications;

internal sealed class ListNotificationsByUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Notifications.ListByUser, async (
            [AsParameters] QueryParams query,
            [AsParameters] Pagination pagination,
            ISender sender) =>
        {
            var result = await sender.Send(new ListNotificationsByUserQuery(
                SharedData.FatherUserId,
                query.UnreadOnly,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Notifications)
        .Produces<ListNotificationsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListNotificationsByUser));
    }

    internal record struct QueryParams(bool UnreadOnly = false);
}