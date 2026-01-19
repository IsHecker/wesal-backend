using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.SchoolReports.UploadSchoolReport;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.SchoolReports;

internal sealed class UploadSchoolReport : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.SchoolReports.Upload, async (
            [FromForm] Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new UploadSchoolReportCommand(
                SharedData.SchoolUserId,
                request.ChildId,
                request.ReportType));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.SchoolReports)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(UploadSchoolReport));
    }

    internal record struct Request(Guid ChildId, string ReportType);
}