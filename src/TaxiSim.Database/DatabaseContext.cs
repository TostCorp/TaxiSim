using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using System.CodeDom.Compiler;
using System.Reflection;

using TaxiSim.Database.BaseModels;
using TaxiSim.Database.Enums;
using TaxiSim.Database.Models;
using TaxiSim.Database.Primitives;

using TostCorp.CollectionExtensions;

namespace TaxiSim.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    internal DbSet<EnumType> Enums { get; init; }

    public DbSet<UserType> UserTypes { get; init; }
    public DbSet<VehicleType> VehicleTypes { get; init; }

    public DbSet<User> Users { get; init; }
    public DbSet<Vehicle> Vehicles { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EnumType>(x =>
        {
            x.ToTable(EnumType.TableName);
            x.HasKey(p => new { p.Id, p.EnumTypeName });
            x.HasIndex(p => p.EnumTypeName);
        });

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            ConfigureVogen(entity, modelBuilder);
            ConfigureEnums(entity, modelBuilder);
            ConfigureNonClusteredPrimaryKey(entity, modelBuilder);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (EF.IsDesignTime)
        {
            optionsBuilder.UseSqlServer(static p => p.UseNetTopologySuite().UseAzureSqlDefaults());
        }

        base.OnConfiguring(optionsBuilder);
    }

    internal static void ConfigureNonClusteredPrimaryKey(IMutableEntityType entity, ModelBuilder modelBuilder)
    {
        if (entity.ClrType.BaseType is { IsGenericType: true } && entity.ClrType.BaseType.GetGenericTypeDefinition() != typeof(BaseEntity<>))
        {
            return;
        }

        modelBuilder.Entity(entity.Name).HasKey(nameof(BaseEntity<int>.Id)).IsClustered(false);
    }

    internal static void ConfigureVogen(IMutableEntityType entity, ModelBuilder modelBuilder)
    {
        if (entity.ClrType.BaseType is not { IsGenericType: true } || entity.ClrType.BaseType.GetGenericTypeDefinition() != typeof(BaseEntity<>))
        {
            return;
        }
        
        foreach (var property in entity.ClrType.GetProperties().FindAll(VogenFilter))
        {
            var type = property.PropertyType.GetNestedType(nameof(UserId.EfCoreValueConverter));
            modelBuilder.Entity(entity.Name).Property(property.Name).HasConversion(type);
        }
    }

    internal static void ConfigureEnums(IMutableEntityType entity, ModelBuilder modelBuilder)
    {
        if (entity.ClrType.BaseType is not { IsGenericType: true } || entity.ClrType.BaseType.GetGenericTypeDefinition() != typeof(EnumType<>))
        {
            return;
        }

        if (entity.ClrType == typeof(EnumType))
        {
            return;
        }

        if (Activator.CreateInstance(entity.ClrType) is not object instance)
        {
            return;
        }

        if (GetEnumTypeName(entity.ClrType) is not MethodInfo enumTypeNameMethodInfo || enumTypeNameMethodInfo.Invoke(instance, null) is not string enumTypeName)
        {
            return;
        }

        if (GetViewName(entity.ClrType) is not MethodInfo viewNameMethodInfo || viewNameMethodInfo.Invoke(instance, null) is not string viewName)
        {
            return;
        }

        modelBuilder.Entity(entity.Name, x =>
        {
            x.ToSqlQuery(QueryString(enumTypeName));
            x.ToView(viewName);
        });
    }

    internal static MethodInfo? GetEnumTypeName(Type type) => type.GetProperty(nameof(EnumType.EnumTypeName), BindingFlags.Public | BindingFlags.Instance)?.GetGetMethod();
    internal static MethodInfo? GetViewName(Type type) => type.GetProperty(nameof(EnumType.ViewName), BindingFlags.Public | BindingFlags.Instance)?.GetGetMethod();

    internal static bool VogenFilter(PropertyInfo x) => x.PropertyType.GetCustomAttribute<GeneratedCodeAttribute>() is GeneratedCodeAttribute { Tool: "Vogen" };
    internal static string QueryString(string filter) => $"SELECT * FROM [{EnumType.TableName}] WHERE [{nameof(EnumType.EnumTypeName)}] = '{filter}'";
}
