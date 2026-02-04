using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.CourtCases.CreateCourtCase;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.CourtCases;

internal sealed class CreateCourtCase : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.CourtCases.Create, async (
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new CreateCourtCaseCommand(
                user.GetRoleId(),
                request.FamilyId,
                request.DocumentId,
                request.CaseNumber,
                request.DecisionSummary));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.CourtCases)
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(CreateCourtCase))
        .RequireAuthorization(CustomPolicies.CourtStaffOnly);
    }

    internal readonly record struct Request(
        Guid FamilyId,
        Guid? DocumentId,
        string CaseNumber,
        string DecisionSummary);
}