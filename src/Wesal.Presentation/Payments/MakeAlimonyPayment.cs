using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Payments.MakeAlimonyPayment;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Payments;

internal sealed class MakeAlimonyPayment : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Payments.MakeAlimony, async (Request request, ISender sender) =>
        {
            var result = await sender.Send(new MakeAlimonyPaymentCommand(
                SharedData.FatherId,
                request.AlimonyId,
                request.PaymentDueId,
                request.Amount,
                request.PaymentMethod));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Payments)
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(MakeAlimonyPayment));
    }

    internal readonly record struct Request(
        Guid AlimonyId,
        Guid PaymentDueId,
        long Amount,
        string PaymentMethod);
}