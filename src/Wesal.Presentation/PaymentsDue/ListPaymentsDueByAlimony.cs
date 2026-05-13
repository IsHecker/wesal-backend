using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Data;
using Wesal.Application.PaymentsDue.ListPaymentsDueByAlimony;
using Wesal.Contracts.Common;
using Wesal.Contracts.PaymentsDue;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.PaymentsDue;

internal sealed class ListPaymentsDueByAlimony : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.PaymentsDue.ListByFamily, async (
            Guid alimonyId,
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListPaymentsDueByAlimonyQuery(
                user.GetRoleId(),
                user.GetRole(),
                alimonyId,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.PaymentsDue)
        .Produces<PagedResponse<PaymentDueResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListPaymentsDueByAlimony))
        .RequireAuthorization(CustomPolicies.CourtAndParents);
    }
}