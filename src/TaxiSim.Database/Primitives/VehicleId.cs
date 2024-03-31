using Vogen;

namespace TaxiSim.Database.Primitives;

[ValueObject<Guid>(Conversions.SystemTextJson | Conversions.EfCoreValueConverter)]
[Instance("Default", "Guid.Empty")]
public readonly partial struct VehicleId
{
    public static bool operator <(VehicleId left, VehicleId right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(VehicleId left, VehicleId right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(VehicleId left, VehicleId right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(VehicleId left, VehicleId right)
    {
        return left.CompareTo(right) >= 0;
    }
}
