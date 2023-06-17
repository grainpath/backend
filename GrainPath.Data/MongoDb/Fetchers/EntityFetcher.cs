using System;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Fetchers;

internal static class EntityFetcher
{
    public static async Task<(Entity, ErrorObject)> Fetch(IMongoDatabase database, string grainId)
    {
        try
        {
            var entity = await database
                .GetCollection<Entity>(MongoDbConst.GRAIN_COLLECTION)
                .Find(grain => grain.grainId == grainId)
                .FirstOrDefaultAsync();

            return (entity, null);
        }
        catch (Exception ex) { return (null, new ErrorObject() { message = ex.Message }); }
    }
}
