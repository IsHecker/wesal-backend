using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Authentication;
using Wesal.Application.Data;
using Wesal.Application.SchoolReports.ListSchoolReportsByChild;
using Wesal.Contracts.Common;
using Wesal.Contracts.SchoolReports;
using Wesal.Domain.Entities.Users;
using Wesal.Presentation.EndpointResults;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.SchoolReports;

internal sealed class ListSchoolReportsByChild : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.SchoolReports.ListByChild, async (
            Guid childId,
            [AsParameters] Pagination pagination,
            ClaimsPrincipal user,
            ISender sender) =>
        {
            var result = await sender.Send(new ListSchoolReportsByChildQuery(
                user.GetRoleId(),
                childId,
                pagination));

            return result.MatchResponse(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.SchoolReports)
        .Produces<PagedResponse<SchoolReportResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithOpenApiName(nameof(ListSchoolReportsByChild))
        .RequireAuthorization();
    }
}