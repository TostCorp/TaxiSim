using TaxiSim.Api.Features.Users.Models;
using TaxiSim.Api.Features.Vehicles.Models;
using TaxiSim.Api.SharedContext;
using TaxiSim.Database.Primitives;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.Users.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class RequestForVehicleService(IMessageQueues messageQueues, IScopedContext scopedContext) : IRequestForVehicleService
{
    public OneOf<SuccessResult, FailureResult> BroadcastRequestForVehicle(BroadcastVehicleRequestDto request)
    {
        var message = new BroadcastVehicleRequestModel
        {
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            VehicleType = request.VehicleType,
            UserId = scopedContext.UserId
        };

        messageQueues.SendMessage(IMessageQueues.VehicleRequests, message);

        return Result.Ok();
    }
}

public interface IRequestForVehicleService
{
    public OneOf<SuccessResult, FailureResult> BroadcastRequestForVehicle(BroadcastVehicleRequestDto request);
}
