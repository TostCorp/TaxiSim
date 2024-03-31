using TaxiSim.Api.SharedContext;
using TaxiSim.Database;
using TaxiSim.Database.Primitives;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.Vehicles.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class DeleteVehicleService(DatabaseContext dbContext, IScopedContext scopedContext) : IDeleteVehicleService
{
    public async Task<OneOf<SuccessResult, FailureResult>> Delete(VehicleId vehicleId, CancellationToken token)
    {
        var dbEntry = await dbContext.Vehicles.FindAsync([vehicleId], token);

        if (dbEntry is null)
        {
            return Result.Fail().WithReason("Vehicle with specified Id does not exist.");
        }

        if (dbEntry.UserId != scopedContext.UserId)
        {
            return Result.Fail().WithReason("Vehicle with specified Id does not exist.");
        }

        dbContext.Vehicles.Remove(dbEntry);

        var result = await dbContext.SaveChangesAsync(token);

        if (result <= 0)
        {
            return Result.Fail().WithReason("Vehicle with specified Id failed to be deleted.");
        }

        return Result.Ok();
    }
}

public interface IDeleteVehicleService
{
    Task<OneOf<SuccessResult, FailureResult>> Delete(VehicleId vehicleId, CancellationToken token);
}