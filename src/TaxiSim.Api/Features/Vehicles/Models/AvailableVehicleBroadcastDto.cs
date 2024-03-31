using TaxiSim.Api.SharedContext;
using TaxiSim.Database.Enums;
using TaxiSim.Database.Primitives;

namespace TaxiSim.Api.Features.Vehicles.Models;

public class AvailableVehicleBroadcastDto : PositionDto
{
    public required VehicleId VehicleId { get; init; }
}