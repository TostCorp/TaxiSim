using NetTopologySuite.Geometries;

using TaxiSim.Database.BaseModels;
using TaxiSim.Database.Enums;
using TaxiSim.Database.Primitives;

namespace TaxiSim.Database.Models;

public sealed class Vehicle : BaseEntity<VehicleId>, ISoftDeleteEntity
{
    public static readonly string TableName = "Vehicles";

    public VehicleTypes VehicleTypeId { get; set; }
    public VehicleType? VehicleType { get; set; }

    public UserId UserId { get; set; }
    public User? User { get; set; }

    public Point Location { get; set; } = default!;

    public VehicleStatuses VehicleStatusId { get; set; }
    public VehicleStatus? VehicleStatus { get; set; }

    bool ISoftDeleteEntity.IsDeleted { get; set; }
    DateTimeOffset? ISoftDeleteEntity.DateDeleted { get; set; }
}
