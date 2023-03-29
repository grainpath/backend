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

    public Task<HeavyPlace> GetPlace(PlaceRequest request) => Place.Act(_database, request);

    public Task<List<FilteredPlace>> GetStack(StackRequest request) => Stack.Act(_database, request);
}
