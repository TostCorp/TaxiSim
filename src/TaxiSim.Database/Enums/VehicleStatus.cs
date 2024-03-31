using System.Text.Json.Serialization;

using TaxiSim.Database.BaseModels;

namespace TaxiSim.Database.Enums;
public sealed class VehicleStatus : EnumType<VehicleStatuses>
{
    public override string ViewName { get; } = "VehicleStatuses";
    public override string EnumTypeName { get; private protected set; } = "VehicleStatuses";
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VehicleStatuses
{
    Offline,
    Available,
    TakingBreak,
    Maintenance,
    Taken
}
