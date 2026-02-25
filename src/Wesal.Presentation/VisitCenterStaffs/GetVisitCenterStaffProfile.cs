using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.VisitCenterStaffs.GetVisitCenterStaffProfile;
using Wesal.Contracts.VisitCenterStaffs;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitCenterStaffs;

internal sealed class GetVisitCenterStaffProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.VisitCenterStaffs.GetProfile, async (
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new GetVisitCenterStaffProfileQuery(user.GetRoleId()));
            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Users)
        .Produces<VisitCenterStaffResponse>(StatusCodes.Status200OK)
        .WithOpenApiName(nameof(GetVisitCenterStaffProfile))
        .RequireAuthorization(CustomPolicies.VisitCenterStaffOnly);
    }
}