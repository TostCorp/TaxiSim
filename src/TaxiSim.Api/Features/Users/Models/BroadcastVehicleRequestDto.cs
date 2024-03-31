using TaxiSim.Api.SharedContext;
using TaxiSim.Database.Enums;

namespace TaxiSim.Api.Features.Users.Models;

public class BroadcastVehicleRequestDto : PositionDto
{
    public required VehicleTypes VehicleType { get; init; }
}
