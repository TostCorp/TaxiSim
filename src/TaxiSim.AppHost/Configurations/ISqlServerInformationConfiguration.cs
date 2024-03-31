namespace TaxiSim.AppHost.Configurations;

public interface ISqlServerInformationConfiguration
{
    public string? Password { get; init; }
    public string? DatabaseName { get; init; }
}
