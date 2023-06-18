using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;
using GrainPath.DataModel.MongoDb.Fetchers;
using MongoDB.Driver;

namespace GrainPath.DataModel.MongoDb;

internal sealed class MongoDbModel : IModel
{
    private readonly IMongoDatabase _database;

    public MongoDbModel(IMongoDatabase database) { _database = database; }

    public AutocsIndex GetAutocs() => AutocsFetcher.Fetch(_database);

    public BoundsObject GetBounds() => BoundsFetcher.Fetch(_database);

    public Task<(Entity, ErrorObject)> GetEntity(string grainId) => EntityFetcher.Fetch(_database, grainId);

    public Task<(List<Place>, ErrorObject)> GetAround(WgsPoint center, double radius, List<Category> categories, int bucket)
        => AroundFetcher.Fetch(_database, center, radius, categories, bucket);

    public Task<(List<Place>, ErrorObject)> GetAroundWithin(List<WgsPoint> polygon, WgsPoint refPoint, double distance, List<Category> categories, int bucket)
        => AroundWithinFetcher.Fetch(_database, polygon, refPoint, distance, categories, bucket);
}
