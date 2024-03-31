namespace TaxiSim.Database.BaseModels;

public interface ISoftDeleteEntity
{
    public bool IsDeleted { get; internal set; }
    public DateTimeOffset? DateDeleted { get; internal set; }

    internal void Delete()
    {
        IsDeleted = true;
        DateDeleted = DateTimeOffset.UtcNow;
    }
}
