using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.PaymentsDue.InitiateAlimonyPayment;
using Wesal.Contracts.PaymentGateway;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.PaymentsDue;

internal sealed class InitiateAlimonyPayment : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.PaymentsDue.InitiateAlimonyPayment, async (
            Guid paymentDueId,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new InitiateAlimonyPaymentCommand(
               user.GetRoleId(),
               paymentDueId));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.PaymentsDue)
        .Produces<PaymentIntentResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(InitiateAlimonyPayment))
        .RequireAuthorization(CustomPolicies.ParentsOnly);
    }
}