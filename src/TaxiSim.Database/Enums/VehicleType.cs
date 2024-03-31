using System.Text.Json.Serialization;

using TaxiSim.Database.BaseModels;

namespace TaxiSim.Database.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VehicleTypes
{
    None,
    Car,
    Bicycle,
    Motorbike
}

public sealed class VehicleType() : EnumType<VehicleTypes>
{
    public override string EnumTypeName { get; private protected set; } = "VehicleTypes";
    public override string ViewName { get; } = "VehicleTypes";
}