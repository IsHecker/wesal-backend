using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Complaints.UpdateComplaintStatus;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Complaints;

internal sealed class UpdateComplaintStatus : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(ApiEndpoints.Complaints.UpdateStatus, async (
            Guid complaintId,
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new UpdateComplaintStatusCommand(
                SharedData.StaffId,
                complaintId,
                request.Status,
                request.ResolutionNotes));

            return result.MatchResponse(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Complaints)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(UpdateComplaintStatus));
    }

    internal readonly record struct Request(string Status, string ResolutionNotes);
}