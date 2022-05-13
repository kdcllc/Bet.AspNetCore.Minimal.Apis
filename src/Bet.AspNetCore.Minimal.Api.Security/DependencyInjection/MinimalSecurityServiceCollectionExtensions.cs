using System.Text;

using Bet.AspNetCore.Minimal.Api.Security.JwtBearer;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection;

public static class MinimalSecurityServiceCollectionExtensions
{
    public static ISecurityAuthenticationBuilder AddSecurity(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "Authentication")
    {
        var defaultAuthenticationScheme = configuration.GetValue<string>($"{sectionName}:DefaultAuthenticationScheme");

        var builder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = defaultAuthenticationScheme;
            options.DefaultChallengeScheme = defaultAuthenticationScheme;
        });

        CheckAddJwtBearer(configuration.GetSection($"{sectionName}:JwtBearer"), builder);

        return new DefaultSecurityAuthenticationBuilder(configuration, builder);
    }

    private static void CheckAddJwtBearer(IConfigurationSection section, AuthenticationBuilder builder)
    {
        var settings = section.Get<JwtBearerSettings>();
        if (settings is null)
        {
            return;
        }

        builder.Services.Configure<JwtBearerSettings>(section);

        builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = settings.Issuers?.Any() ?? false,
                ValidIssuers = settings.Issuers,
                ValidateAudience = settings.Audiences?.Any() ?? false,
                ValidAudiences = settings.Audiences,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = !string.IsNullOrWhiteSpace(settings.SecurityKey),
                IssuerSigningKey = !string.IsNullOrWhiteSpace(settings.SecurityKey)
                    ? new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecurityKey))
                    : null,
                RequireExpirationTime = settings.ExpirationTime.GetValueOrDefault() > TimeSpan.Zero,
                ClockSkew = settings.ClockSkew
            };
        });

        if (settings.EnableJwtBearerGeneration)
        {
            builder.Services.TryAddSingleton<IJwtBearerGeneratorService, JwtBearerGeneratorService>();
        }
    }
}
