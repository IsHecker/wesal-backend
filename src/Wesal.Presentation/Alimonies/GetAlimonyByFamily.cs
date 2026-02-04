using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Alimonies.GetAlimonyByFamily;
using Wesal.Application.Authentication;
using Wesal.Contracts.Alimonies;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Alimonies
{
    internal sealed class GetAlimonyByFamily : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet(ApiEndpoints.Alimonies.GetByFamily, async (
                Guid familyId,
                ClaimsPrincipal user,
                ISender sender) =>
            {
                var result = await sender.Send(new GetAlimonyByFamilyQuery(user.GetCourtId(), familyId));

                return result.MatchResponse(Results.Ok, ApiResults.Problem);
            })
            .WithTags(Tags.Alimonies)
            .Produces<AlimonyResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithOpenApiName(nameof(GetAlimonyByFamily))
            .RequireAuthorization(CustomPolicies.CourtStaffOnly);
        }
    }
}