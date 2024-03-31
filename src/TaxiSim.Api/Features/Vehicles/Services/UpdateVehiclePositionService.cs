using NetTopologySuite.Geometries;

using TaxiSim.Api.Features.Vehicles.Models;
using TaxiSim.Database;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.Vehicles.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class UpdateVehiclePositionService(DatabaseContext dbContext) : IUpdateVehiclePositionService
{
    public async Task<OneOf<SuccessResult, FailureResult>> UpdatePosition(AvailableVehicleBroadcastDto request, CancellationToken token)
    {
        var dbVehicle = await dbContext.Vehicles.FindAsync([request.VehicleId], token);
        if (dbVehicle is null)
        {
            return Result.Fail().WithReason("Vehicle position can not be updated.");
        }

        var dbUser = await dbContext.Users.FindAsync([dbVehicle.UserId], token);
        if (dbUser is null)
        {
            return Result.Fail().WithReason("Vehicle position can not be updated.");
        }

        dbVehicle.Location = new Point(request.Latitude, request.Longitude);
        dbContext.Vehicles.Update(dbVehicle);

        dbUser.Location = new Point(request.Latitude, request.Longitude);
        dbContext.Users.Update(dbUser);

        var result = await dbContext.SaveChangesAsync(token);
        if (result <= 0)
        {
            return Result.Fail().WithReason("Vehicle update failed.");
        }

        return Result.Ok();
    }
}

public interface IUpdateVehiclePositionService
{
    Task<OneOf<SuccessResult, FailureResult>> UpdatePosition(AvailableVehicleBroadcastDto request, CancellationToken token);
}