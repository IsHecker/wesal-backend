using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Schools.RegisterSchool;
using Wesal.Contracts.Schools;
using Wesal.Domain;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Schools;

internal sealed class RegisterSchool : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Schools.Register, async (
            Request request,
            ISender sender) =>
        {
            var result = await sender.Send(new RegisterSchoolCommand(
                SharedData.StaffUserId,
                request.Name,
                request.Address,
                request.ContactNumber));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Schools)
        .Produces<RegisterSchoolResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .WithOpenApiName(nameof(RegisterSchool));
    }

    internal record struct Request(
        string Name,
        string Address,
        string Governorate,
        string? ContactNumber);
}