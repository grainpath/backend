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

    public AutocIndex GetAutoc() => Autoc.Act(_database);

    public BoundObject GetBound() => Bound.Act(_database);

    public Task<HeavyPlace> GetPlace(string id) => Place.Act(_database, id);

    public Task<List<FilteredPlace>> GetAround(WgsPoint center, double radius, List<KeywordCondition> conditions)
        => Around.Act(_database, center, radius, conditions);

    public Task<List<FilteredPlace>> GetWithin(List<WgsPoint> polygon, List<KeywordCondition> conditions)
        => Within.Act(_database, polygon, conditions);
}
