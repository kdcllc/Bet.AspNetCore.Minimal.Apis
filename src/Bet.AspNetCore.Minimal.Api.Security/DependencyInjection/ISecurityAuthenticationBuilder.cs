using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public interface ISecurityAuthenticationBuilder
{
    public IConfiguration Configuration { get; }

    public AuthenticationBuilder Builder { get; }
}
