using TaxiSim.Database;
using TaxiSim.Database.BaseModels;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.SimpleCrudOperations.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class SimpleReadService<T, TU>(DatabaseContext dbContext) : ISimpleReadService<T, TU> where T : BaseEntity<TU> where TU : struct
{
    public async Task<OneOf<SuccessResult<T>, FailureResult>> Read(TU value, CancellationToken token)
    {
        var result = await dbContext.Set<T>().FindAsync([value], token);

        if (result is null)
        {
            return Result.Fail().WithReason("Object with specified Id was not found.");
        }

        return Result.Ok(result);
    }
}

public interface ISimpleReadService<T, TU> where T : BaseEntity<TU> where TU : struct
{
    Task<OneOf<SuccessResult<T>, FailureResult>> Read(TU value, CancellationToken token);
}