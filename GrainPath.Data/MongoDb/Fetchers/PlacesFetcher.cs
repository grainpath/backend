using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Data.MongoDb.Helpers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Fetchers;

internal static class PlacesFetcher
{
    /// <summary>
    /// Fetch at most <c>bucket</c> places for each category. A found place
    /// exactly matches the corresponding category.
    /// </summary>
    /// <param name="baseFilter">Base filter definition.</param>
    public static async Task<List<Place>> Fetch(
        IMongoDatabase database, FilterDefinition<Entity> baseFilter, List<Category> categories, int bucket)
    {
        var result = new Dictionary<string, Place>();

        for (int i = 0; i < categories.Count; ++i)
        {
            // Request database to obtain a list of places.

            var docs = await database
                .GetCollection<Entity>(MongoDbConst.GRAIN_COLLECTION)
                .Find(baseFilter & FilterConstructor.CategoryToFilter(categories[i]))
                .Project(Builders<Entity>.Projection.Exclude(p => p.linked).Exclude(p => p.attributes).Exclude(p => p.position))
                .Limit(bucket)
                .ToListAsync();

            // Convert BSON documents to proper places, add category identifier.

            var places = docs
                .Select(d => BsonSerializer.Deserialize<Place>(d))
                .Select(p => { _ = p.selected.Add(i); return p; });

            /* Merge the list of places with the result in case the same place
             * is associated with more than one category. */

            foreach (var p in places)
            {
                if (result.TryGetValue(p.grainId, out var place))
                {
                    place.selected.Add(i);
                }
                else { result.Add(place.grainId, place); }
            }
        }

        return result.Values.ToList();
    }
}
