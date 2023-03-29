using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Actions;

internal static class Place
{
    public static Task<HeavyPlace> Act(IMongoDatabase database, string id)
    {
        return database
            .GetCollection<HeavyPlace>(MongoDbConst.GRAIN_COLLECTION)
            .Find(grain => grain.id == id)
            .FirstOrDefaultAsync();
    }
}
