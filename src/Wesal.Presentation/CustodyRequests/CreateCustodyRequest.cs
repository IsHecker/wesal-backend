using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.CustodyRequests.CreateCustodyRequest;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.CustodyRequests;

internal sealed class CreateCustodyRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.CustodyRequests.Create, async (
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new CreateCustodyRequestCommand(
                SharedData.FatherId,
                request.StartDate,
                request.EndDate,
                request.Reason));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CustodyRequests)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(CreateCustodyRequest));
    }

    internal record struct Request(
        DateOnly StartDate,
        DateOnly EndDate,
        string Reason);
}