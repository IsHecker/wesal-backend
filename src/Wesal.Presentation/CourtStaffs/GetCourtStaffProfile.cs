using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.CourtStaffs.GetCourtStaffProfile;
using Wesal.Contracts.CourtStaffs;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.CourtStaffs;

internal sealed class GetCourtStaffProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.CourtStaffs.GetById, async (
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new GetCourtStaffProfileQuery(user.GetRoleId()));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CourtStaffs)
        .Produces<CourtStaffResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(GetCourtStaffProfile))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }
}