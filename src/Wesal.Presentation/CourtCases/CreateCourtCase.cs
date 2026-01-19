using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.CourtCases.CreateCourtCase;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.CourtCases;

internal sealed class CreateCourtCase : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.CourtCases.Create, async (Request request, ISender sender) =>
        {
            var result = await sender.Send(new CreateCourtCaseCommand(
                SharedData.StaffUserId,
                request.FamilyId,
                request.CaseNumber,
                request.Status,
                request.DecisionSummary));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CourtCases)
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(CreateCourtCase));
    }

    internal readonly record struct Request(
        Guid FamilyId,
        string CaseNumber,
        string Status,
        string DecisionSummary);
}