using System.Text.Json.Serialization;

using TaxiSim.Database.BaseModels;

namespace TaxiSim.Database.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserTypes
{
    None,
    Customer,
    Driver,
    InternalEmployee
}

public sealed class UserType : EnumType<UserTypes>
{
    public override string ViewName { get; } = "UserTypes";
    public override string EnumTypeName { get; private protected set; } = "UserTypes";
}