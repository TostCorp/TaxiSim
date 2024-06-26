﻿using Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using System.Reflection.Metadata.Ecma335;

using TaxiSim.ServiceDefaults.Attributes;

namespace TaxiSim.ServiceDefaults;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        var assembly = Assembly.GetEntryAssembly();
        ArgumentNullException.ThrowIfNull(assembly);

        return services.ConfigureDependencyInjectionWithAssemblyRef(assembly);
    }

    public static IServiceCollection ConfigureDependencyInjectionWithAssemblyRef(this IServiceCollection services, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assembly);

        assembly.GetTypes()
            .Configure(services, ServiceLifetime.Singleton)
            .Configure(services, ServiceLifetime.Scoped)
            .Configure(services, ServiceLifetime.Transient);

        return services;
    }

    private static Type[] Configure(this Type[] types, IServiceCollection services, ServiceLifetime lifetime)
    {
        foreach (var implementation in Array.FindAll(types, p => p.GetCustomAttribute<LifetimeAttribute>(false)?.ServiceLifetime == lifetime))
        {
            var interfaces = implementation.GetInterfaces();
            if (interfaces.Length is 0)
            {
                services.Add(new(implementation, implementation, lifetime));
                continue;
            }

            foreach (var @interface in interfaces)
            {
                if (implementation is { IsGenericType: true } && @interface is { IsGenericType: true })
                {
                    services.Add(new(@interface.GetGenericTypeDefinition(), implementation.GetGenericTypeDefinition(), lifetime));
                }

                if (implementation is { IsGenericType: false } && @interface is { IsGenericType: true })
                {
                    services.Add(new(@interface.GetGenericTypeDefinition(), implementation, lifetime));
                }

                if (implementation is { IsGenericType: true } && @interface is { IsGenericType: false })
                {
                    services.Add(new(@interface, implementation.GetGenericTypeDefinition(), lifetime));
                }

                if (implementation is { IsGenericType: false } && @interface is { IsGenericType: false })
                {
                    services.Add(new(@interface, implementation, lifetime));
                }
            }
        }

        return types;
    }
}
