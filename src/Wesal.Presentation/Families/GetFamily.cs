using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Families.GetFamily;
using Wesal.Contracts.Families;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Families;

internal sealed class GetFamily : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Families.GetById, async (Guid familyId, ClaimsPrincipal user, ISender sender) =>
        {
            var result = await sender.Send(new GetFamilyQuery(user.GetCourtId(), familyId));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Families)
        .Produces<FamilyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .WithOpenApiName(nameof(GetFamily))
        .RequireAuthorization(CustomPolicies.CourtStaffOnly);
    }
}