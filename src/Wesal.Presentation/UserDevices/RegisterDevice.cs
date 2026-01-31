using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.UserDevices.RegisterDevice;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.UserDevices;

internal sealed class RegisterDevice : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.UserDevices.RegisterDevice, async (
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new RegisterDeviceCommand(
                SharedData.FatherId,
                request.DeviceToken,
                request.Platform));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.UserDevices)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(RegisterDevice));
    }

    internal record struct Request(string DeviceToken, string Platform);
}