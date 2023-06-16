using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Fetchers;

internal static class EntityFetcher
{
    public static Task<Entity> Fetch(IMongoDatabase database, string grainId)
    {
        return database
            .GetCollection<Entity>(MongoDbConst.GRAIN_COLLECTION)
            .Find(grain => grain.grainId == grainId)
            .FirstOrDefaultAsync();
    }
}
