using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public class DefaultSecurityAuthenticationBuilder : ISecurityAuthenticationBuilder
{
    public DefaultSecurityAuthenticationBuilder(IConfiguration configuration, AuthenticationBuilder builder)
    {
        Configuration = configuration;
        Builder = builder;
    }

    public IConfiguration Configuration { get; }

    public AuthenticationBuilder Builder { get; }
}
