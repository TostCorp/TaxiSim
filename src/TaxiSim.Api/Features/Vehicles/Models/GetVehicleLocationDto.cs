using TaxiSim.Database.Primitives;

namespace TaxiSim.Api.Features.Vehicles.Models;

public class GetVehicleLocationDto
{
    public required VehicleId VehicleId { get; set; }
}
