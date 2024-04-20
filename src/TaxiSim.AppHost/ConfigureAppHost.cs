using Microsoft.Extensions.Configuration;

using TaxiSim.AppHost.Configurations;
using TaxiSim.ServiceDefaults;

namespace TaxiSim.AppHost;

public static class ConfigureAppHost
{
    public static IDistributedApplicationBuilder Configure(this IDistributedApplicationBuilder builder)
    {
        var database = builder.InitializeSqlServer();
        var rabbitMq = builder.InitializeRabbitMQ();

        builder.AddProject<Projects.TaxiSim_Api>(ProjectNames.TaxiSimApiName).WithReference(database).WithReference(rabbitMq);

        return builder;
    }

    private static IResourceBuilder<SqlServerDatabaseResource> InitializeSqlServer(this IDistributedApplicationBuilder builder)
    {
        var sqlServerConfiguration = builder.Configuration.GetSection(SqlServerConfiguration.SectionName).Get<SqlServerConfiguration>();
        ArgumentNullException.ThrowIfNull(sqlServerConfiguration);

        var password = builder.AddParameter("SqlServer-Password", true);

        return builder.AddSqlServer(ConnectionStrings.Db, password, sqlServerConfiguration.Port)
                                                         .WithImageTag(sqlServerConfiguration.Tag)
                                                         .WithImage(sqlServerConfiguration.Image)
                                                         .WithImageRegistry(sqlServerConfiguration.Registry ?? string.Empty)
                                                         .WithDataVolume()
                                                         .AddDatabase(DatabaseNames.TaxiSimDatabase, sqlServerConfiguration.DatabaseName);
    }

    private static IResourceBuilder<RabbitMQServerResource> InitializeRabbitMQ(this IDistributedApplicationBuilder builder)
    {
        return builder.AddRabbitMQ(ConnectionStrings.RabbitMq)
                      .WithDataVolume()
                      .WithVolume("rabbitmq-volume-log", "/var/log/rabbitmq");
    }
}
