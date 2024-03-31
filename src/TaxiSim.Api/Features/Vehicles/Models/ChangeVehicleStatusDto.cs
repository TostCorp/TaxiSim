using TaxiSim.Database.Enums;
using TaxiSim.Database.Primitives;

namespace TaxiSim.Api.Features.Vehicles.Models;

public class ChangeVehicleStatusDto
{
    public required VehicleId VehicleId { get; init; }
    public required VehicleStatuses VehicleStatus { get; init; }
}
