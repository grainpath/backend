using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace GrainPath.Data.MongoDb.Fetchers;

internal static class AroundFetcher
{
    /// <summary>
    /// Fetch points ordered by spheric distance to the center.
    /// <list>
    /// <item>https://www.mongodb.com/docs/manual/reference/operator/query/nearSphere/</item>
    /// </list>
    /// </summary>
    public static async Task<(List<Place>, ErrorObject)> Fetch(
        IMongoDatabase database, WgsPoint center, double radius, List<Category> categories, int bucket)
    {
        var sphereFilter = Builders<Entity>.Filter
            .NearSphere(p => p.position, GeoJson.Point(new GeoJson2DGeographicCoordinates(center.lon, center.lat)), maxDistance: radius);

        return await PlacesFetcher.Fetch(database, sphereFilter, categories, bucket);
    }
}
