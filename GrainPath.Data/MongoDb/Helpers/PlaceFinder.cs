using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Helpers;

internal static class PlaceFinder
{
    public static async Task<List<FilteredPlace>> Fetch(IMongoDatabase database, FilterDefinition<HeavyPlace> basef, List<KeywordCondition> conditions, int limit)
    {
        var r = new Dictionary<string, FilteredPlace>();

        foreach (var cond in conditions)
        {
            var docs = await database
                .GetCollection<HeavyPlace>(MongoDbConst.GRAIN_COLLECTION)
                .Find(basef & FilterConstructor.ConditionToFilter(cond))
                .Project(Builders<HeavyPlace>.Projection.Exclude(p => p.linked).Exclude(p => p.attributes).Exclude(p => p.position))
                .Limit(limit)
                .ToListAsync();

            var items = docs
                .Select(d => BsonSerializer.Deserialize<LightPlace>(d))
                .Select(p => new FilteredPlace() { place = p, satisfy = new() { cond.keyword } });

            foreach (var item in items)
            {
                if (r.TryGetValue(item.place.grainId, out var val))
                {
                    val.satisfy.Add(cond.keyword);
                }
                else { r.Add(item.place.grainId, item); }
            }
        }

        return r.Values.ToList();
    }
}
