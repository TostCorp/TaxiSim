using TaxiSim.Database;
using TaxiSim.Database.BaseModels;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.SimpleCrudOperations.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class SimpleCreateService<T>(DatabaseContext dbContext) : ISimpleCreateService<T> where T : BaseEntity
{
    public async Task<OneOf<SuccessResult, FailureResult>> Create(T[] value, CancellationToken token)
    {
        dbContext.Set<T>().AddRange(value);

        var result = await dbContext.SaveChangesAsync(token);

        if (result <= 0)
        {
            return Result.Fail().WithReason("Object failed to be created.");
        }

        return Result.Ok();
    }
}

public interface ISimpleCreateService<T> where T : BaseEntity
{
    Task<OneOf<SuccessResult, FailureResult>> Create(T[] value, CancellationToken token);
}