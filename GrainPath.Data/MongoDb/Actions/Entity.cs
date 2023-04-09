using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Actions;

internal static class EntityAction
{
    public static Task<Entity> Act(IMongoDatabase database, string placeId)
    {
        return database
            .GetCollection<Entity>(MongoDbConst.GRAIN_COLLECTION)
            .Find(grain => grain.placeId == placeId)
            .FirstOrDefaultAsync();
    }
}
