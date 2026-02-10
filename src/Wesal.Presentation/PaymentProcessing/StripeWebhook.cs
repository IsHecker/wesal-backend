using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.PaymentProcessing;

internal sealed class StripeWebhook : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.PaymentProcessing.StripeWebhook, async (
            HttpContext context,
            IStripeEventDispatcher dispatcher,
            [FromQuery] bool isConnect = false) =>
        {
            var json = await new StreamReader(context.Request.Body).ReadToEndAsync();
            var stripeSignature = context.Request.Headers["Stripe-Signature"];

            var result = await dispatcher.DispatchAsync(json, stripeSignature!, isConnect);

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.PaymentProcessing)
        .WithOpenApiName(nameof(StripeWebhook));
    }
}