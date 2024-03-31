namespace TaxiSim.AppHost;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        builder.Configure();

        await using var app = builder.Build();

        await app.RunAsync();
    }
}
