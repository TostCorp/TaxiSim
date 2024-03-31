namespace TaxiSim.AppHost.Configurations;

public interface IDockerContainerInformationConfiguration
{
    public string Image { get; init; }
    public string Tag { get; init; }
    public string? Registry { get; init; }
    public int? Port { get; init; }
}
