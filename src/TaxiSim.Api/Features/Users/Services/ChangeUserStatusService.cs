using TaxiSim.Api.Features.Users.Models;
using TaxiSim.Api.SharedContext;
using TaxiSim.Database;
using TaxiSim.Database.Primitives;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.Users.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class ChangeUserStatusService(DatabaseContext dbContext, IScopedContext scopedContext) : IChangeUserStatusService
{
    public async Task<OneOf<SuccessResult, FailureResult>> ChangeStatus(ChangeUserStatusDto request, CancellationToken token)
    {
        var dbData = await dbContext.Users.FindAsync([scopedContext.UserId.Value], token);

        if (dbData is null)
        {
            return Result.Fail().WithReason("User not found.");
        }

        dbData.UserStatusId = request.UserStatus;

        dbContext.Users.Update(dbData);

        var result = await dbContext.SaveChangesAsync(token);

        if (result <= 0)
        {
            return Result.Fail().WithReason("User failed to update.");
        }

        return Result.Ok();
    }
}

public interface IChangeUserStatusService
{
    Task<OneOf<SuccessResult, FailureResult>> ChangeStatus(ChangeUserStatusDto request, CancellationToken token);
}
