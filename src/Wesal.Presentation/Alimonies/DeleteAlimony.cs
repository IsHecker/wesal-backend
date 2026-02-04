using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Alimonies.DeleteAlimony;
using Wesal.Application.Authentication;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Alimonies;

internal sealed class DeleteAlimony : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(ApiEndpoints.Alimonies.Delete, async (
            Guid alimoneyId,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new DeleteAlimonyCommand(user.GetCourtId(), alimoneyId));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Alimonies)
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(DeleteAlimony))
        .RequireAuthorization(CustomPolicies.CourtStaffOnly);
    }
}