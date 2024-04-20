using System.ComponentModel.DataAnnotations;

namespace TaxiSim.Database.BaseModels;

public abstract class BaseEntity<T> : BaseEntity where T : struct
{
    [Key]
    public T Id { get; init; }
}

public abstract class BaseEntity : ITimeTrackedEntity, IConcurrentEntity
{
    DateTimeOffset ITimeTrackedEntity.DateCreated { get; set; }
    DateTimeOffset? ITimeTrackedEntity.DateModified { get; set; }

    [Timestamp]
    byte[] IConcurrentEntity.RowVersion { get; set; } = default!;

    internal void Create()
    {
        ((ITimeTrackedEntity)this).DateCreated = DateTimeOffset.UtcNow;
    }

    internal void Modify()
    {
        ((ITimeTrackedEntity)this).DateModified = DateTimeOffset.UtcNow;
    }
}
