namespace TaxiSim.AppHost.Configurations;

public class SqlServerConfiguration : IDockerContainerInformationConfiguration, ISqlServerInformationConfiguration
{
    public static readonly string SectionName = "SqlServer";

    public required string Image { get; init; }
    public required string Tag { get; init; }
    public required int? Port { get; init; }
    public required string? Registry { get; init; }
    public required string? Password { get; init; }
    public required string? DatabaseName { get; init; }
}
