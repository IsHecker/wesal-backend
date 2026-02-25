using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Children.AddChild;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Children;

internal sealed class AddChild : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Families.Children, async (
            Guid familyId,
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new AddChildCommand(
                user.GetCourtId(),
                familyId,
                request.SchoolId,
                request.FullName,
                request.BirthDate,
                request.Gender));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Children)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(AddChild))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }

    internal record struct Request(
        string FullName,
        DateOnly BirthDate,
        string Gender,
        Guid? SchoolId);
}