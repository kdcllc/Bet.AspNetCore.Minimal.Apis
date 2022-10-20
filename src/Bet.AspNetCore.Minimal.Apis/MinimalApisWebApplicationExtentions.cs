using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// <see cref="WebApplication"/> extension methods for minimal apis.
/// </summary>
public static class MinimalApisWebApplicationExtentions
{
    /// <summary>
    /// Uses configuration flag 'HttpsRedirection' to use ssl or not.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseHttpsRedirect(this WebApplication app)
    {
        var enableHttpsRedirection = app.Configuration.GetValue<bool>("HttpsRedirection");
        if (enableHttpsRedirection)
        {
            app.UseHttpsRedirection();
        }

        return app;
    }

    /// <summary>
    /// Adds default healthchecks, headers forwarding.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddContainerSupport(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();

        // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-3.1
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });

        return builder;
    }
}
