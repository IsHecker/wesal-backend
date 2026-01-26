using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Complaints.CreateComplaint;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Complaints;

internal sealed class CreateComplaint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Complaints.Create, async (Request request, ISender sender) =>
        {
            var result = await sender.Send(new CreateComplaintCommand(
                SharedData.FatherId,
                request.Type,
                request.Description));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Complaints)
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(CreateComplaint));
    }

    internal readonly record struct Request(string Type, string Description);
}