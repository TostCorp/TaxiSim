using TaxiSim.Database.Enums;

namespace TaxiSim.Api.Features.Users.Models;

public class GetUserLocationResponse
{
    public required double Latitude { get; init; }
    public required double Longitude { get; init; }
}
