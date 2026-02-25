using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Custodies.CreateCustody;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Custodies;

internal sealed class CreateCustody : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Custodies.Create, async (
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new CreateCustodyCommand(
                user.GetCourtId(),
                request.CourtCaseId,
                request.CustodialParentId,
                request.StartAt,
                request.EndAt));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Custodies)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(CreateCustody))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }

    internal record struct Request(
        Guid CourtCaseId,
        Guid CustodialParentId,
        DateTime StartAt,
        DateTime? EndAt);
}