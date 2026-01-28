using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Documents.UploadDocument;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Documents;

internal sealed class UploadDocument : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Documents.Upload, async ([FromForm] Request request, ISender sender) =>
        {
            var result = await sender.Send(new UploadDocumentCommand(SharedData.StaffId, request.File));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Documents)
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithOpenApiName(nameof(UploadDocument))
        .DisableAntiforgery();
    }

    internal record struct Request(IFormFile File = default!);
}