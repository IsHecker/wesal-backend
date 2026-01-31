using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.UserDevices.UnregisterDevice;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.UserDevices;

internal sealed class UnregisterDevice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.UserDevices.UnregisterDevice, async (
            string deviceToken,
            ISender sender) =>
        {
            var result = await sender.Send(new UnregisterDeviceCommand(
                SharedData.FatherId,
                deviceToken));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.UserDevices)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(UnregisterDevice));
    }
}