using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
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
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListPaymentsDueByFamilyQuery(
                user.GetRoleId(),
                user.GetRole(),
                familyId,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.PaymentsDue)
        .Produces<PagedResponse<PaymentDueResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListPaymentsDueByFamily))
        .RequireAuthorization(CustomPolicies.ParentsOnly)
        .RequireAuthorization(CustomPolicies.CourtStaffOnly);
    }
}