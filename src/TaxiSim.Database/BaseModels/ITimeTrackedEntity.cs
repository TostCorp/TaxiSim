namespace TaxiSim.Database.BaseModels;

public interface ITimeTrackedEntity
{
    public DateTimeOffset DateCreated { get; internal set; }
    public DateTimeOffset? DateModified { get; internal set; }
}