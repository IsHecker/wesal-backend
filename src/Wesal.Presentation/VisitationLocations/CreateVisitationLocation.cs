using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.VisitationLocations.CreateVisitationLocation;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitationLocations;

internal sealed class CreateVisitationLocation : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.VisitationLocations.Create, async (
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new CreateVisitationLocationCommand(
                user.GetRoleId(),
                request.Name,
                request.Address,
                request.Governorate,
                request.ContactNumber,
                request.MaxConcurrentVisits,
                request.OpeningTime,
                request.ClosingTime));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.VisitationLocations)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .WithOpenApiName(nameof(CreateVisitationLocation))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }

    internal sealed record Request(
        string Name,
        string Address,
        string Governorate,
        string? ContactNumber,
        int MaxConcurrentVisits,
        TimeOnly OpeningTime,
        TimeOnly ClosingTime);
}