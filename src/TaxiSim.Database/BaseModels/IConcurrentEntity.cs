namespace TaxiSim.Database.BaseModels;

public interface IConcurrentEntity
{
    public byte[] RowVersion { get; internal set; }
}
