using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Alimonies.GetAlimonyByCourtCase;
using Wesal.Application.Authentication;
using Wesal.Contracts.Alimonies;
using Wesal.Domain.Entities.Users;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Alimonies;

internal sealed class GetAlimonyByCourtCase : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Alimonies.GetByCourtCase, async (
            Guid courtCaseId,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new GetAlimonyByCourtCaseQuery(
                user.GetRoleId(),
                user.GetCourtId(),
                courtCaseId,
                user.GetRole() == UserRole.Parent));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Alimonies)
        .Produces<AlimonyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(GetAlimonyByCourtCase))
        .RequireAuthorization(CustomPolicies.CourtAndParents);
    }
}