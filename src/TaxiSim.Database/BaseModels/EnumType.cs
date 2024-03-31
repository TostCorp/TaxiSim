using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using TaxiSim.Database.Constants;

namespace TaxiSim.Database.BaseModels;

public abstract class EnumType<T> : BaseEntity where T : unmanaged
{
    public T Id { get; init; }

    [MaxLength(EnumTypeConstants.NameLength)]
    public required string Name { get; set; }

    [MaxLength(EnumTypeConstants.EnumTypeNameLength)]
    public abstract string EnumTypeName { get; private protected set; }

    [NotMapped, JsonIgnore, XmlIgnore]
    public abstract string ViewName { get; }
}

/// <summary>
/// Used only for Table creation
/// </summary>
internal sealed class EnumType : EnumType<int>
{
    internal static string TableName = "Enums";

    public override string ViewName { get; } = string.Empty;
    public override string EnumTypeName { get; private protected set; } = string.Empty;

    internal void SetEnumTypeName(string value)
    {
        EnumTypeName = value;
    }
}