using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.FamilyCourts.DeleteFamilyCourt;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.FamilyCourts;

internal sealed class DeleteFamilyCourt : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Courts.Delete, async (
            Guid courtId,
            ISender sender) =>
        {
            var result = await sender.Send(new DeleteFamilyCourtCommand(courtId));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Courts)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(DeleteFamilyCourt))
        .RequireAuthorization(CustomPolicies.SystemAdminOnly);
    }
}