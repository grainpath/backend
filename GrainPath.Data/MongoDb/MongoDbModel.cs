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

    public Dictionary<string, AutocompleteIndex> GetAutocomplete() => Autocomplete.Act(_database);

    public Task<HeavyPlace> Find(string id)
    {
        return _database
            .GetCollection<HeavyPlace>(MongoDbConst.GrainCollection)
            .Find(grain => grain.id == id)
            .FirstOrDefaultAsync();
    }
}
