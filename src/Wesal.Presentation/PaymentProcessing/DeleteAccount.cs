using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Abstractions.PaymentGateway;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.PaymentProcessing;

internal sealed class DeleteAccount : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/stripe/delete-account{accountId}", async (
            string accountId,
            IStripeGateway stripeGateway) =>
        {
            await stripeGateway.DeleteAccount(accountId);
        })
        .WithTags(Tags.PaymentProcessing)
        .WithOpenApiName(nameof(DeleteAccount));
    }
}