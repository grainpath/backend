using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Actions;

internal static class Place
{
    public static Task<HeavyPlace> Act(IMongoDatabase database, PlaceRequest request)
    {
        return database
            .GetCollection<HeavyPlace>(MongoDbConst.GrainCollection)
            .Find(grain => grain.id == request.id)
            .FirstOrDefaultAsync();
    }
}
