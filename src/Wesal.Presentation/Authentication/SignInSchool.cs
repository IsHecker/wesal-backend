using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication.Credentials;
using Wesal.Application.Authentication.SignIn;
using Wesal.Contracts.Authentication;
using Wesal.Domain.Entities.Users;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Authentication;

internal sealed class SignInSchool : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Auth.SignInSchool, async (Request request, ISender sender) =>
        {
            var credentials = new UsernamePasswordCredentials(request.Username, request.Password);
            var result = await sender.Send(new SignInCommand<UsernamePasswordCredentials>(
                credentials,
                UserRole.School));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Authentication)
        .Produces<JwtTokenResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithOpenApiName(nameof(SignInSchool))
        .AllowAnonymous();
    }

    internal record struct Request(string Username, string Password);
}