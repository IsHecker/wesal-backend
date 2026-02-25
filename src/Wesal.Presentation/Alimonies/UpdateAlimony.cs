using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Alimonies.UpdateAlimony;
using Wesal.Application.Authentication;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Alimonies;

internal sealed class UpdateAlimony : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Alimonies.Update, async (
            Guid alimoneyId,
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateAlimonyCommand(
                user.GetCourtId(),
                alimoneyId,
                request.Amount,
                request.Frequency,
                request.StartDate,
                request.EndDate));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Alimonies)
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(UpdateAlimony))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }

    internal record struct Request(
        long Amount,
        string Frequency,
        DateOnly StartDate,
        DateOnly? EndDate);
}