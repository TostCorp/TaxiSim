namespace TaxiSim.Api.Features.Vehicles.Models;

public class GetVehicleLocationResponse
{
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}
