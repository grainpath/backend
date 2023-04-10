using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Helpers;

internal static class PlaceFinder
{
    public static async Task<List<Place>> Fetch(IMongoDatabase database, FilterDefinition<Entity> basef, List<KeywordCondition> conditions, int limit)
    {
        var r = new Dictionary<string, Place>();

        foreach (var cond in conditions)
        {
            var ds = await database
                .GetCollection<Entity>(MongoDbConst.GRAIN_COLLECTION)
                .Find(basef & FilterConstructor.ConditionToFilter(cond))
                .Project(Builders<Entity>.Projection.Exclude(p => p.linked).Exclude(p => p.attributes).Exclude(p => p.position))
                .Limit(limit)
                .ToListAsync();

            var ps = ds
                .Select(d => BsonSerializer.Deserialize<Place>(d))
                .Select(p => { _ = p.selected.Add(cond.keyword); return p; });

            foreach (var p in ps)
            {
                if (r.TryGetValue(p.placeId, out var place))
                {
                    place.selected.Add(cond.keyword);
                }
                else { r.Add(p.placeId, p); }
            }
        }

        return r.Values.ToList();
    }
}
