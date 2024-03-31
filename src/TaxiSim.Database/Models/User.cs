
using NetTopologySuite.Geometries;

using TaxiSim.Database.BaseModels;
using TaxiSim.Database.Enums;
using TaxiSim.Database.Primitives;

namespace TaxiSim.Database.Models;

public class User : BaseEntity<UserId>, ISoftDeleteEntity
{
    public UserTypes UserTypeId { get; set; }
    public UserType? UserType { get; set; }

    public Point Location { get; set; } = default!;

    public UserStatuses UserStatusId { get; set; }
    public UserStatus? UserStatus { get; set; }

    bool ISoftDeleteEntity.IsDeleted { get; set; }
    DateTimeOffset? ISoftDeleteEntity.DateDeleted { get; set; }

    public ICollection<Vehicle>? Vehicles { get; set; }
}
