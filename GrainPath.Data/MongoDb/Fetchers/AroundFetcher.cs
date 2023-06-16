using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace GrainPath.Data.MongoDb.Fetchers;

internal static class AroundFetcher
{
    private readonly static int MIN_BUCKET_SIZE = 25;
    private readonly static int MAX_RESPONSE_SIZE = 100;

    /// <summary>
    /// Fetch points ordered by spheric distance to the center.
    /// <list>
    /// <item>https://www.mongodb.com/docs/manual/reference/operator/query/nearSphere/</item>
    /// </list>
    /// </summary>
    public static async Task<List<Place>> Fetch(IMongoDatabase database, WgsPoint center, double radius, List<Category> categories)
    {
        var limit = Math.Max(MIN_BUCKET_SIZE, MAX_RESPONSE_SIZE / categories.Count);

        var sphereFilter = Builders<Entity>.Filter
            .NearSphere(p => p.position, GeoJson.Point(new GeoJson2DGeographicCoordinates(center.lon, center.lat)), maxDistance: radius);

        return await PlacesFetcher.Fetch(database, sphereFilter, categories, limit);
    }
}
