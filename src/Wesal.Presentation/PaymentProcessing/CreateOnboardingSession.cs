using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.PaymentProcessing.CreateOnboardingSession;
using Wesal.Contracts.PaymentGateway;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.PaymentProcessing;

internal sealed class CreateOnboardingSession : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.PaymentProcessing.CreateOnboardingSession, async (
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new CreateOnboardingSessionCommand(
                user.GetRoleId(),
                request.RefreshUrl,
                request.ReturnUrl));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.PaymentProcessing)
        .Produces<SessionResponse>(StatusCodes.Status200OK)
        .WithOpenApiName(nameof(CreateOnboardingSession));
    }

    internal readonly record struct Request(string RefreshUrl, string ReturnUrl);
}