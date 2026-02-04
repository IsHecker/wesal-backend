using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Alimonies.CreateAlimony;
using Wesal.Application.Authentication;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Alimonies;

internal sealed class CreateAlimony : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Alimonies.Create, async (
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var command = new CreateAlimonyCommand(
                user.GetCourtId(),
                request.CourtCaseId,
                request.PayerId,
                request.RecipientId,
                request.Amount,
                request.Frequency,
                request.StartDate,
                request.EndDate);

            var result = await sender.Send(command);

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Alimonies)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(CreateAlimony))
        .RequireAuthorization(CustomPolicies.CourtStaffOnly);
    }

    internal record struct Request(
        Guid CourtCaseId,
        Guid PayerId,
        Guid RecipientId,
        long Amount,
        string Frequency,
        DateOnly StartDate,
        DateOnly EndDate);
}