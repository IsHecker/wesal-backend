using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Schools.GetSchoolProfile;
using Wesal.Contracts.Schools;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Schools;

internal sealed class GetSchoolProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Schools.GetProfile, async (
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new GetSchoolProfileQuery(user.GetRoleId()));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Schools)
        .Produces<SchoolResponse>(StatusCodes.Status200OK)
        .WithOpenApiName(nameof(GetSchoolProfile))
        .RequireAuthorization(CustomPolicies.SchoolsOnly);
    }
}