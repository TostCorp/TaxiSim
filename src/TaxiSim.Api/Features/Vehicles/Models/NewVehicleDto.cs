using TaxiSim.Api.SharedContext;
using TaxiSim.Database.Enums;

namespace TaxiSim.Api.Features.Vehicles.Models;

public class NewVehicleDto : PositionDto
{
    public required VehicleTypes VehicleType { get; init; }
}