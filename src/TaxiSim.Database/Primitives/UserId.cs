using Vogen;

namespace TaxiSim.Database.Primitives;

[ValueObject<Guid>(Conversions.SystemTextJson | Conversions.EfCoreValueConverter)]
[Instance("Default", "Guid.Empty")]
public readonly partial struct UserId
{
    public static bool operator <(UserId left, UserId right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(UserId left, UserId right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(UserId left, UserId right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(UserId left, UserId right)
    {
        return left.CompareTo(right) >= 0;
    }
}
