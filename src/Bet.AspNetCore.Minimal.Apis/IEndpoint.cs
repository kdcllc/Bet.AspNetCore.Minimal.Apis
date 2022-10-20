using Microsoft.AspNetCore.Routing;

namespace Bet.AspNetCore.Minimal.Apis;

public interface IEndpoint
{
    void MapRoutes(IEndpointRouteBuilder routes);
}
