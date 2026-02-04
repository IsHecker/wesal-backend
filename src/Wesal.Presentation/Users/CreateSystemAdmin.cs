using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.SystemAdmins.CreateSystemAdmin;
using Wesal.Contracts.Users;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Users;

internal sealed class CreateSystemAdmin : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Users.CreateSystemAdmin, async (
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new CreateSystemAdminCommand(
                request.Email,
                request.FullName));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Users)
        .Produces<UserCredentialResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .WithOpenApiName(nameof(CreateSystemAdmin))
        .AllowAnonymous();
    }

    internal record struct Request(string Email, string FullName);
}