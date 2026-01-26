using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.CustodyRequests.ProcessCustodyRequest;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.CustodyRequests;

internal sealed class ProcessCustodyRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(ApiEndpoints.CustodyRequests.Process, async (
            Guid requestId,
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new ProcessCustodyRequestCommand(
                SharedData.StaffId,
                requestId,
                request.IsApproved,
                request.DecisionNote));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.CustodyRequests)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ProcessCustodyRequest));
    }

    internal record struct Request(
        bool IsApproved,
        string DecisionNote);
}