using TaxiSim.Api.Features.Vehicles.Models;
using TaxiSim.Database;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.Vehicles.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class ChangeVehicleStatusService(DatabaseContext dbContext) : IChangeVehicleStatusService
{
    public async Task<OneOf<SuccessResult, FailureResult>> ChangeStatus(ChangeVehicleStatusDto request, CancellationToken token)
    {
        var dbData = await dbContext.Vehicles.FindAsync([request.VehicleId], token);
        if (dbData is null)
        {
            return Result.Fail().WithReason("Vehicle not found.");
        }

        dbData.VehicleStatusId = request.VehicleStatus;

        dbContext.Vehicles.Update(dbData);

        var result = await dbContext.SaveChangesAsync(token);

        if (result <= 0)
        {
            return Result.Fail().WithReason("Vehicle failed to update.");
        }

        return Result.Ok();
    }
}

public interface IChangeVehicleStatusService
{
    Task<OneOf<SuccessResult, FailureResult>> ChangeStatus(ChangeVehicleStatusDto request, CancellationToken token);
}