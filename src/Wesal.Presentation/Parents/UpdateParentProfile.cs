using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Parents.UpdateParentProfile;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Parents;

internal sealed class UpdateParentProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Parents.UpdateProfile, async (
            Guid parentId,
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateParentProfileCommand(
                user.GetCourtId(),
                parentId,
                request.Email,
                request.Phone,
                request.Address,
                request.Job));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Parents)
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(UpdateParentProfile))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }

    internal record struct Request(
        string? Email,
        string Phone,
        string Address,
        string? Job);
}