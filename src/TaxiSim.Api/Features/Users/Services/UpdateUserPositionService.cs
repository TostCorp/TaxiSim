using TaxiSim.Api.SharedContext;
using TaxiSim.Database;
using TaxiSim.Database.Primitives;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.Users.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class UpdateUserPositionService(DatabaseContext dbContext, IScopedContext scopedContext) : IUpdateUserPositionService
{
    public async Task<OneOf<SuccessResult, FailureResult>> UpdatePosition(PositionDto request, CancellationToken token)
    {
        var dbData = await dbContext.Users.FindAsync([scopedContext.UserId.Value], token);

        if (dbData is null)
        {
            return Result.Fail().WithReason("User not found.");
        }

        dbData.Location = new NetTopologySuite.Geometries.Point(request.Latitude, request.Longitude);

        dbContext.Users.Update(dbData);

        var result = await dbContext.SaveChangesAsync(token);

        if (result <= 0)
        {
            return Result.Fail().WithReason("User failed to update.");
        }

        return Result.Ok();
    }
}

public interface IUpdateUserPositionService
{
    Task<OneOf<SuccessResult, FailureResult>> UpdatePosition(PositionDto request, CancellationToken token);
}