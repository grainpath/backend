using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Data.MongoDb.Helpers;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace GrainPath.Data.MongoDb.Actions;

internal static class WithinAction
{
    public static async Task<List<SelectedPlace>> Act(IMongoDatabase database, List<WgsPoint> polygon, WgsPoint centroid, double distance, List<KeywordCondition> conditions)
    {
        var limit = Math.Max(MongoDbConst.BUCKET_SIZE, MongoDbConst.REQUEST_SIZE / conditions.Count);

        var base1 = Builders<Entity>.Filter
            .NearSphere(p => p.position, GeoJson.Point(new GeoJson2DGeographicCoordinates(centroid.lon, centroid.lat)), maxDistance: distance);

        var base2 = Builders<Entity>.Filter
            .GeoWithin(p => p.position, GeoJson.Polygon(polygon.Select(point => new GeoJson2DGeographicCoordinates(point.lon, point.lat)).ToArray()));

        var basef = base1 & base2;

        return await PlaceFinder.Fetch(database, basef, conditions, limit);
    }
}
