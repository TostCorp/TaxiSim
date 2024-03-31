using TaxiSim.Database;
using TaxiSim.Database.BaseModels;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.SimpleCrudOperations.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class SimpleUpdateService<T, TU>(DatabaseContext dbContext, ISimpleReadService<T, TU> readService) : ISimpleUpdateService<T, TU> where T : BaseEntity<TU> where TU : struct
{
    public async Task<OneOf<SuccessResult, FailureResult>> Update(TU id, Func<T, T> transform, CancellationToken token)
    {
        var dbEntryAsIResult = await readService.Read(id, token);

        if (dbEntryAsIResult.TryPickT1(out var failure, out var successResult))
        {
            return failure;
        }

        transform.Invoke(successResult.Value);

        dbContext.Set<T>().Update(successResult.Value);

        var result = await dbContext.SaveChangesAsync(token);
        if (result <= 0)
        {
            return Result.Fail().WithReason("Failed to update the provided Id.");
        }

        return Result.Ok();
    }
}

public interface ISimpleUpdateService<T, TU> where T : BaseEntity<TU> where TU : struct
{
    Task<OneOf<SuccessResult, FailureResult>> Update(TU id, Func<T, T> transform, CancellationToken token);
}