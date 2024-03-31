using TaxiSim.Api.Features.Users.Models;
using TaxiSim.Database;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.Users.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class GetUserPositionService(DatabaseContext dbContext) : IGetUserPositionService
{
    public async Task<OneOf<SuccessResult<GetUserLocationResponse>, FailureResult>> Fetch(GetUserLocationDto request, CancellationToken token)
    {
        var dbData = await dbContext.Users.FindAsync([request.UserId], token);
        if (dbData is null)
        {
            return Result.Fail().WithReason("User not found.");
        }

        var mapped = new GetUserLocationResponse
        {
            Latitude = dbData.Location.X,
            Longitude = dbData.Location.Y,
        };

        return Result.Ok(mapped);
    }
}

public interface IGetUserPositionService
{
    Task<OneOf<SuccessResult<GetUserLocationResponse>, FailureResult>> Fetch(GetUserLocationDto request, CancellationToken token);
}