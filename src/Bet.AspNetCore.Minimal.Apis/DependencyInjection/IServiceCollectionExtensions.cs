﻿using Bet.AspNetCore.Minimal.Apis;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var endpoints = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t => t.GetInterfaces().Contains(typeof(IEndpoint)))
            .Where(t => !t.IsInterface);

        foreach (var endpoint in endpoints)
        {
            services.AddScoped(typeof(IEndpoint), endpoint);
        }

        return services;
    }
}
