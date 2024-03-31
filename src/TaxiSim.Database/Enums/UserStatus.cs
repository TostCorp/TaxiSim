using System.Text.Json.Serialization;

using TaxiSim.Database.BaseModels;

namespace TaxiSim.Database.Enums;

public class UserStatus : EnumType<UserStatuses>
{
    public override string ViewName { get; } = "UserStatuses";
    public override string EnumTypeName { get; private protected set; } = "UserStatuses";
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserStatuses
{
    Offline,
    Online,
    RequestingVehicle,
    WaitingForVehicle,
    OnRide,
    Working,
    OnBreak
}