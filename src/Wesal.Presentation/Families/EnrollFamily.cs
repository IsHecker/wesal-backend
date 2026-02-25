using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Families.EnrollFamily;
using Wesal.Application.Families.EnrollFamily.Dtos;
using Wesal.Contracts.Families;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Families;

internal sealed class EnrollFamily : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Families.Enroll, async (
            Request request,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new EnrollFamilyCommand(
                user.GetCourtId(),
                request.Father.ToDto(),
                request.Mother.ToDto(),
                request.Children?.Select(c => c.ToDto())));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Families)
        .Produces<EnrollFamilyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .WithOpenApiName(nameof(EnrollFamily))
        .RequireAuthorization(CustomPolicies.CourtManagement);
    }

    internal readonly record struct Request(
        CreateParentRequest Father,
        CreateParentRequest Mother,
        List<CreateChildRequest>? Children);

    internal readonly record struct CreateParentRequest(
        string NationalId,
        string FullName,
        DateOnly BirthDate,
        string Gender,
        string? Job,
        string Address,
        string Phone,
        string? Email)
    {
        internal CreateParentDto ToDto() => new(
            NationalId,
            FullName,
            BirthDate,
            Address,
            Phone,
            Job,
            Email);
    }

    internal readonly record struct CreateChildRequest(
        string FullName,
        DateOnly BirthDate,
        string Gender,
        Guid? SchoolId)
    {
        internal CreateChildDto ToDto() => new(
            FullName,
            BirthDate,
            Gender,
            SchoolId
        );
    }
}