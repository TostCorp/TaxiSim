using Microsoft.Extensions.DependencyInjection;

namespace TaxiSim.ServiceDefaults.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class LifetimeAttribute(ServiceLifetime serviceLifetime) : Attribute
{
    public ServiceLifetime ServiceLifetime { get; } = serviceLifetime;
}
