using System.ComponentModel.DataAnnotations;

namespace TaxiSim.Database.BaseModels;

public abstract class BaseEntity<T> : BaseEntity where T : struct
{
    [Key]
    public T Id { get; init; }
}

public abstract class BaseEntity : IBaseEntity
{
    DateTimeOffset IBaseEntity.DateCreated { get; set; }
    DateTimeOffset? IBaseEntity.DateModified { get; set; }

    [Timestamp]
    byte[] IBaseEntity.RowVersion { get; set; } = default!;

    internal void Create()
    {
        ((IBaseEntity)this).DateCreated = DateTimeOffset.UtcNow;
    }

    internal void Modify()
    {
        ((IBaseEntity)this).DateModified = DateTimeOffset.UtcNow;
    }
}

public interface IBaseEntity
{
    public DateTimeOffset DateCreated { get; internal set; }
    public DateTimeOffset? DateModified { get; internal set; }

    public byte[] RowVersion { get; internal set; }
}