using NetTopologySuite.Geometries;

using TaxiSim.Api.Features.Vehicles.Models;
using TaxiSim.Api.SharedContext;
using TaxiSim.Database;
using TaxiSim.Database.Models;
using TaxiSim.Database.Primitives;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.Vehicles.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class NewVehicleService(DatabaseContext dbContext, IScopedContext scopedContext) : INewVehicleService
{
    public async Task<OneOf<SuccessResult, FailureResult>> Create(NewVehicleDto request, CancellationToken token)
    {
        var newVehicle = new Vehicle
        {
            Id = VehicleId.Default,
            Location = new Point(request.Latitude, request.Longitude) { SRID = 4326 },
            VehicleTypeId = request.VehicleType,
            User = new() { Id = scopedContext.UserId, UserTypeId = Database.Enums.UserTypes.Driver },
            UserId = UserId.Default
        };

        dbContext.Vehicles.Add(newVehicle);

        var result = await dbContext.SaveChangesAsync(token);

        if (result <= 0)
        {
            return Result.Fail().WithHttpStatusCode(System.Net.HttpStatusCode.BadRequest);
        }

        return Result.Ok();
    }
}

public interface INewVehicleService
{
    public Task<OneOf<SuccessResult, FailureResult>> Create(NewVehicleDto request, CancellationToken token);
}
