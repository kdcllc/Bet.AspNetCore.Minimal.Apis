namespace Microsoft.AspNetCore.Builder;

// ideas: https://github.com/dotnet/aspnetcore/issues/36007#issuecomment-981401208
public static class OpenApiExtensions
{
    public static RouteHandlerBuilder IncludeInOpenApi(this RouteHandlerBuilder builder, string groupName)
    {
        builder.Add(endpointBuilder => endpointBuilder.Metadata.Add(new IncludeOpenApi(groupName)));
        return builder;
    }
}

public interface IIncludeOpenApi
{
    public string GroupName { get; }
}

public class IncludeOpenApi : IIncludeOpenApi
{
    public IncludeOpenApi(string groupName)
    {
        GroupName = groupName;
    }

    public string GroupName { get; }
}
