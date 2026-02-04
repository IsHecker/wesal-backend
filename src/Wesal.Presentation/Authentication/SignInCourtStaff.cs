using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication.Credentials;
using Wesal.Application.Authentication.SignIn;
using Wesal.Contracts.Authentication;
using Wesal.Domain.Entities.Users;
using Wesal.Presentation.Authentication.Requests;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Authentication;

internal sealed partial class SignInCourtStaff : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Auth.SignInCourtStaff, async (EmailPasswordRequest request, ISender sender) =>
        {
            var credentials = new EmailPasswordCredentials(request.Email, request.Password);
            var result = await sender.Send(new SignInCommand<EmailPasswordCredentials>(
                credentials,
                UserRole.CourtStaff));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Authentication)
        .Produces<JwtTokenResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithOpenApiName(nameof(SignInCourtStaff))
        .AllowAnonymous();
    }
}