using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Abstractions.Services;
using Wesal.Domain;
using Wesal.Domain.Entities.Notifications;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Notifications;

internal sealed class TestNotification : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("test-notification", async (INotificationService notificationService) =>
        {
            // var notification = await notificationService.SendNotificationAsync(NotificationTemplate.PaymentDue(SharedData.FatherId, 1500.00m, DateTime.UtcNow.AddHours(5)));

            return Results.Ok();
        })
        .WithTags(Tags.Notifications)
        .Produces<int>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(TestNotification));
    }
}