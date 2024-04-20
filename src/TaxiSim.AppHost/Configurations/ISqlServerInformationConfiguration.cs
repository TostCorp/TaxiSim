namespace TaxiSim.AppHost.Configurations;

public interface ISqlServerInformationConfiguration
{
    public string? DatabaseName { get; init; }
}
