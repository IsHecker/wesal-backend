using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.CourtStaffs.CreateCourtStaff;
using Wesal.Contracts.Users;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Users;
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
            if (user.GetRole() == nameof(StaffRole.Manager) &&
                request.Role.Equals(nameof(StaffRole.Manager), StringComparison.OrdinalIgnoreCase))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status403Forbidden,
                    title: "Forbidden",
                    detail: "Only Family Court Administrators can create Manager staff accounts.");
            }

            var result = await sender.Send(new CreateCourtStaffCommand(
                user.GetCourtId(),
                request.Email,
                request.FullName,
                request.Role,
                request.Phone));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Users)
        .Produces<UserCredentialResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .WithOpenApiName(nameof(CreateCourtStaff))
        .RequireAuthorization(CustomPolicies.CourtAdminOnly);
    }

    internal record struct Request(
        string Email,
        string FullName,
        string Role,
        string? Phone);
}