using Microsoft.EntityFrameworkCore;

using TaxiSim.Database;
using TaxiSim.ServiceDefaults;
using TaxiSim.Database.Interceptors;
using TaxiSim.Api.Middleware;
using TaxiSim.Api.Swagger.SchemaFilters;

namespace TaxiSim.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SchemaFilter<VogenSchemaFilter>();
        });

        builder.Services.ConfigureDependencyInjection();

        builder.AddSqlServerDbContext<DatabaseContext>(
            ConnectionStrings.Db,
            static x => x.ConnectionString = Environment.GetEnvironmentVariable($"ConnectionStrings__{DatabaseNames.TaxiSimDatabase}"),
            static x =>
            {
                x.AddInterceptors(new SoftDeleteInterceptor(), new AddOrModifyInterceptor(), new ConcurrentInterceptor());
                x.UseSqlServer(p => p.UseNetTopologySuite().UseAzureSqlDefaults());
            });

        builder.AddRabbitMQClient(ConnectionStrings.RabbitMq);

        await using var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        await using (var scope = app.Services.CreateAsyncScope())
        await using (var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>())
        {
            await db.Database.MigrateAsync(app.Lifetime.ApplicationStopping);
            await db.RunEFSyncEnumService(app.Lifetime.ApplicationStopping);
        }

        app.UseScopedContext();

        await app.RunAsync();
    }
}
