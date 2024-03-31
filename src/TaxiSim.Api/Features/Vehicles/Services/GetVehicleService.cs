using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

using TaxiSim.Api.Features.Vehicles.Models;
using TaxiSim.Database;
using TaxiSim.Database.Models;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.CollectionExtensions;
using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.Vehicles.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class GetVehicleService(DatabaseContext dbContext) : IGetVehicleService
{
    public async Task<SuccessResult<GetVehiclesResponse>> Fetch(VehicleRequestDto request, CancellationToken token)
    {
        var result = await dbContext.Vehicles.Where(p => p.Location.Within(request.RequestCircle.Value)).TagWithCallSite().ToArrayAsync(token);
        var mapped = result.ConvertAll(MapToVehicleInfo);

        var response = new GetVehiclesResponse() { Vehicles = mapped };

        return Result.Ok(response);
    }

    internal static VehicleInfo MapToVehicleInfo(Vehicle vehicle)
    {
        return new VehicleInfo { Latitude = vehicle.Location.X, Longitude = vehicle.Location.Y, Type = vehicle.VehicleTypeId, VehicleId = vehicle.Id };
    }
}

public interface IGetVehicleService
{
    Task<SuccessResult<GetVehiclesResponse>> Fetch(VehicleRequestDto request, CancellationToken token);
}
