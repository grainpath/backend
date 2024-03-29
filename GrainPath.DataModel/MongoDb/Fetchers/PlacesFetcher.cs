using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.DataModel.MongoDb.Helpers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace GrainPath.DataModel.MongoDb.Fetchers;

internal static class PlacesFetcher
{
    /// <summary>
    /// Fetch at most <c>bucket</c> places for each category. A found place
    /// exactly matches the corresponding category.
    /// </summary>
    /// <param name="baseFilter">Base filter definition.</param>
    public static async Task<(List<Place>, ErrorObject)> Fetch(
        IMongoDatabase database, FilterDefinition<Entity> baseFilter, List<Category> categories, int bucket)
    {
        try
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
                    .Select(p => { _ = p.categories.Add(i); return p; });

                /* Merge the list of places with the result in case the same place
                * is associated with more than one category. */

                foreach (var place in places)
                {
                    if (result.TryGetValue(place.grainId, out var p))
                    {
                        p.categories.Add(i);
                    }
                    else { result.Add(place.grainId, place); }
                }
            }

            return (result.Values.ToList(), null);
        }
        catch (Exception ex) { return (null, new() { Message = ex.Message }); }
    }
}
