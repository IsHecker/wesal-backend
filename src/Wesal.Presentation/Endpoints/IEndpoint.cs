using Microsoft.AspNetCore.Routing;

namespace Wesal.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}