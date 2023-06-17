using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace GrainPath.Data.MongoDb.Fetchers;

internal static class AroundWithinFetcher
{
    /// <summary>
    /// Combine <c>$geoWithin</c> and <c>$nearSphere</c> filters to ensure points are within
    /// the polygon and closed to the center of the circle. Typically the circle is circumscribed
    /// about the polygon, and the polygon is a bounding ellipse.
    /// <list>
    /// <item>https://www.mongodb.com/docs/manual/reference/operator/query/geoWithin/</item>
    /// <item>https://www.mongodb.com/docs/manual/reference/operator/query/nearSphere/</item>
    /// </list>
    /// </summary>
    public static async Task<(List<Place>, ErrorObject)> Fetch(
        IMongoDatabase database, List<WgsPoint> polygon, WgsPoint refPoint, double distance, List<Category> categories, int bucket)
    {
        var sphereFilter = Builders<Entity>.Filter
            .NearSphere(p => p.position, GeoJson.Point(new GeoJson2DGeographicCoordinates(refPoint.lon, refPoint.lat)), maxDistance: distance);

        var withinFilter = Builders<Entity>.Filter
            .GeoWithin(p => p.position, GeoJson.Polygon(polygon.Select(point => new GeoJson2DGeographicCoordinates(point.lon, point.lat)).ToArray()));

        return await PlacesFetcher.Fetch(database, sphereFilter & withinFilter, categories, bucket);
    }
}
