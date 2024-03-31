using TaxiSim.Database.Enums;
using TaxiSim.Database.Primitives;

namespace TaxiSim.Api.Features.Vehicles.Models;

public class GetVehiclesResponse
{
    public required VehicleInfo[] Vehicles { get; init; }
}

public class VehicleInfo
{
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
    public required VehicleTypes Type { get; init; }
    public required VehicleId VehicleId { get; init; }
}
