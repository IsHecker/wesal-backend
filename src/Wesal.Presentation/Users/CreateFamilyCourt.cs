using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.FamilyCourts.CreateFamilyCourt;
using Wesal.Contracts.Users;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Users;

internal sealed class CreateFamilyCourt : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Users.CreateFamilyCourt, async (
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new CreateFamilyCourtCommand(
                request.Email,
                request.Name,
                request.Governorate,
                request.Address,
                request.ContactInfo));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Users)
        .Produces<UserCredentialResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .WithOpenApiName(nameof(CreateFamilyCourt))
        .RequireAuthorization(CustomPolicies.SystemAdminOnly);
    }

    internal record struct Request(
        string Email,
        string Name,
        string Governorate,
        string Address,
        string ContactInfo);
}