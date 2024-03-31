using TaxiSim.Database.Enums;
using TaxiSim.Database.Primitives;

namespace TaxiSim.Api.Features.Vehicles.Models;

public class BroadcastVehicleRequestModel
{
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
    public required VehicleTypes VehicleType { get; init; }
    public required UserId UserId { get; init; }
}
