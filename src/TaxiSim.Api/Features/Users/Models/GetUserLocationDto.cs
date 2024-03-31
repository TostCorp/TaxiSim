using TaxiSim.Database.Primitives;

namespace TaxiSim.Api.Features.Users.Models;

public class GetUserLocationDto
{
    public required UserId UserId { get; init; }
}
