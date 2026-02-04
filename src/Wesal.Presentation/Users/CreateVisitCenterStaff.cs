using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.VisitCenterStaffs.CreateVisitCenterStaff;
using Wesal.Contracts.Users;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Users;

internal sealed class CreateVisitCenterStaff : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Users.CreateVisitCenterStaff, async (
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var command = new CreateVisitCenterStaffCommand(
                user.GetCourtId(),
                request.LocationId,
                request.Email,
                request.FullName,
                request.Phone);

            var result = await sender.Send(command);

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Users)
        .Produces<UserCredentialResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .WithOpenApiName(nameof(CreateVisitCenterStaff))
        .RequireAuthorization(CustomPolicies.CourtStaffOnly);
    }

    internal record struct Request(
        Guid LocationId,
        string Email,
        string FullName,
        string? Phone);
}
