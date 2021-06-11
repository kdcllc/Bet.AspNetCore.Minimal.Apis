using System.Reflection;

using Microsoft.AspNetCore.Mvc.Routing;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapRoutes<T>(this IEndpointRouteBuilder routes)
    {
        // This will create an optimized factory that resolves constructor arguments from the DI container
        var objectFactory = ActivatorUtilities.CreateFactory(typeof(T), Type.EmptyTypes);

        foreach (var method in typeof(T).GetMethods())
        {
            var attribute = method.GetCustomAttribute<HttpMethodAttribute>(inherit: true);

            if (attribute is null)
            {
                continue;
            }

            // Create a delegate mapping the route template and methods
            var del = RequestDelegateFactory.Create(method, context => objectFactory(context.RequestServices, null));

            routes.MapMethods(attribute?.Template ?? "/", attribute?.HttpMethods ?? new List<string> { "HttpGetAttribute" }, del);
        }

        return routes;
    }
}
