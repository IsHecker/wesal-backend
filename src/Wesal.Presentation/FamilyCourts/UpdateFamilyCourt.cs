using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.FamilyCourts.UpdateFamilyCourt;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.FamilyCourts;

internal sealed class UpdateFamilyCourt : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(ApiEndpoints.Courts.Update, async (
            Guid courtId,
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateFamilyCourtCommand(
                courtId,
                request.Email,
                request.Name,
                request.Governorate,
                request.Address,
                request.ContactInfo));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Courts)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(UpdateFamilyCourt))
        .RequireAuthorization();
    }

    internal record struct Request(
        string Email,
        string Name,
        string Governorate,
        string Address,
        string ContactInfo);
}