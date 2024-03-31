using TaxiSim.Database;
using TaxiSim.Database.BaseModels;
using TaxiSim.ServiceDefaults.Attributes;

using TostCorp.ObjectResults.Core;

namespace TaxiSim.Api.Features.SimpleCrudOperations.Services;

[Lifetime(ServiceLifetime.Scoped)]
public class SimpleDeleteService<T, TU>(DatabaseContext dbContext, ISimpleReadService<T, TU> readService) : ISimpleDeleteService<T, TU> where T : BaseEntity<TU> where TU : struct
{
    public async Task<OneOf<SuccessResult, FailureResult>> Delete(TU value, CancellationToken token)
    {
        var dbEntryAsIResult = await readService.Read(value, token);
        if (dbEntryAsIResult.TryPickT1(out var failure, out var successResult))
        {
            return failure;
        }

        dbContext.Set<T>().Remove(successResult.Value);

        var result = await dbContext.SaveChangesAsync(token);

        if (result is <= 0)
        {
            return Result.Fail().WithReason($"Failed to delete object with Id {value}");
        }

        return Result.Ok();
    }
}

public interface ISimpleDeleteService<T, TU> where T : BaseEntity<TU> where TU : struct
{
    Task<OneOf<SuccessResult, FailureResult>> Delete(TU value, CancellationToken token);
}