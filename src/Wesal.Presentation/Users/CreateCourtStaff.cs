using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.CourtStaffs.CreateCourtStaff;
using Wesal.Contracts.Users;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Users;

internal sealed class CreateCourtStaff : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Users.CreateCourtStaff, async (
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new CreateCourtStaffCommand(
                user.GetCourtId(),
                request.Email,
                request.FullName,
                request.Phone));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Users)
        .Produces<UserCredentialResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .WithOpenApiName(nameof(CreateCourtStaff))
        .RequireAuthorization(CustomPolicies.FamilyCourtAdminOnly);
    }

    internal record struct Request(
        string Email,
        string FullName,
        string? Phone);
}