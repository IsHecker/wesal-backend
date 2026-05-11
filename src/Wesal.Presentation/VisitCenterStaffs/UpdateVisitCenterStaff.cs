using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.VisitCenterStaffs.UpdateVisitCenterStaff;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.VisitCenterStaffs;

internal sealed class UpdateVisitCenterStaff : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.VisitCenterStaffs.Update, async (
            Guid staffId,
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateVisitCenterStaffCommand(
                user.GetCourtId(),
                staffId,
                request.FullName,
                request.Phone));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.VisitCenterStaffs)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(UpdateVisitCenterStaff))
        .RequireAuthorization(CustomPolicies.CourtAdminOnly);
    }

    internal readonly record struct Request(
        string FullName,
        string? Phone);
}