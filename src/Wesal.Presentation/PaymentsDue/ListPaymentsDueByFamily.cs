using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Data;
using Wesal.Application.PaymentsDue.ListPaymentsDueByFamily;
using Wesal.Contracts.Common;
using Wesal.Contracts.PaymentsDue;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.PaymentsDue;

internal sealed class ListPaymentsDueByFamily : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.PaymentsDue.ListByFamily, async (
            Guid familyId,
            [AsParameters] Pagination pagination,
            ISender sender) =>
        {
            var result = await sender.Send(new ListPaymentsDueByFamilyQuery(familyId, pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.PaymentsDue)
        .Produces<PagedResponse<PaymentDueResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListPaymentsDueByFamily));
    }
}