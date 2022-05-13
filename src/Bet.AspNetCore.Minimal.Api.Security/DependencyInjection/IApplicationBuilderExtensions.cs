namespace Microsoft.AspNetCore.Builder;

public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSecurity(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        return app;
    }
}
