using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using TaxiSim.Database.BaseModels;

using TostCorp.CollectionExtensions;

namespace TaxiSim.Database;

public static class EFEnumSyncService
{
    public static async Task RunEFSyncEnumService(this DatabaseContext dbContext, CancellationToken token)
    {
        var dbEntries = await GetFromDatabase(dbContext, token);
        var localEntries = GetFromLocal();

        var newEntries = GetNewEntries(dbEntries, localEntries);
        var updatedEntries = GetUpdatedEntries(dbEntries, localEntries);
        var deletedEntries = GetDeletedEnumTypes(dbEntries, localEntries);

        if (token.IsCancellationRequested)
        {
            return;
        }

        dbContext.Enums.RemoveRange(deletedEntries);
        dbContext.Enums.AddRange(newEntries);
        dbContext.Enums.UpdateRange(updatedEntries);

        await dbContext.SaveChangesAsync(token);
    }

    internal static EnumType[] GetDeletedEnumTypes(EnumType[] dbEntries, EnumType[] localEntries)
    {
        return dbEntries.FindAll(p => !localEntries.Exists(x => x.Id == p.Id && x.EnumTypeName == p.EnumTypeName));
    }

    internal static EnumType[] GetUpdatedEntries(EnumType[] dbEntries, EnumType[] localEntries)
    {
        return localEntries.FindAll(p => dbEntries.Exists(x => x.Id == p.Id && x.EnumTypeName == p.EnumTypeName && x.Name != p.Name));
    }

    internal static async Task<EnumType[]> GetFromDatabase(DatabaseContext dbContext, CancellationToken token)
    {
        return await dbContext.Enums.ToArrayAsync(token);
    }

    internal static EnumType[] GetNewEntries(EnumType[] dbEntries, EnumType[] localEntries)
    {
        return localEntries.FindAll(p => !dbEntries.Exists(x => x.Id == p.Id && x.EnumTypeName == p.EnumTypeName));
    }

    internal static EnumType[] GetFromLocal()
    {
        var localEnums = typeof(DatabaseContext).Assembly.GetTypes().FindAll(p => p.IsEnum).ConvertAll(p =>
        {
            var enumValues = Enum.GetValues(p);
            var arr = new EnumType[enumValues.Length];

            for (int i = 0; i < enumValues.Length; i++)
            {
                var enumValue = enumValues.GetValue(i);
                var value = new EnumType { Name = enumValue!.ToString()!, Id = (int)enumValue };
                value.SetEnumTypeName(p.Name);

                arr[i] = value;
            }

            return arr;
        });

        var localEnumArr = localEnums.SelectMany(p => p);

        return localEnumArr.ToArray();
    }
}
