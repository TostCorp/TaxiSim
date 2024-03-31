using TaxiSim.Api.Features.Vehicles.Models;
using TaxiSim.Database;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.Vehicles.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class GetVehiclePositionService(DatabaseContext dbContext) : IGetVehiclePositionService
{
    public async Task<OneOf<SuccessResult<GetVehicleLocationResponse>, FailureResult>> Fetch(GetVehicleLocationDto request, CancellationToken token)
    {
        var dbData = await dbContext.Vehicles.FindAsync([request.VehicleId], token);
        if (dbData is null)
        {
            return Result.Fail().WithReason("Vehicle not found.");
        }

        var mapped = new GetVehicleLocationResponse { Latitude = dbData.Location.X, Longitude = dbData.Location.Y };

        return Result.Ok(mapped);
    }
}

public interface IGetVehiclePositionService
{
    Task<OneOf<SuccessResult<GetVehicleLocationResponse>, FailureResult>> Fetch(GetVehicleLocationDto request, CancellationToken token);
}