using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;
using GrainPath.Data.MongoDb.Actions;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb;

internal sealed class MongoDbModel : IModel
{
    private readonly IMongoDatabase _database;

    public MongoDbModel(IMongoDatabase database) { _database = database; }

    public AutocsIndex GetAutocs() => AutocsAction.Act(_database);

    public BoundsObject GetBounds() => BoundsAction.Act(_database);

    public Task<Entity> GetEntity(string placeId) => EntityAction.Act(_database, placeId);

    public Task<List<SelectedPlace>> GetAround(WgsPoint center, double radius, List<KeywordCondition> conditions)
        => AroundAction.Act(_database, center, radius, conditions);

    public Task<List<SelectedPlace>> GetNearestWithin(List<WgsPoint> polygon, WgsPoint centroid, double distance, List<KeywordCondition> conditions)
        => WithinAction.Act(_database, polygon, centroid, distance, conditions);
}
