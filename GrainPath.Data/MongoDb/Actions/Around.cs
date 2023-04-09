using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Data.MongoDb.Helpers;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace GrainPath.Data.MongoDb.Actions;

internal static class AroundAction
{
    public static async Task<List<FilteredPlace>> Act(IMongoDatabase database, WgsPoint center, double radius, List<KeywordCondition> conditions)
    {
        var limit = Math.Max(MongoDbConst.BUCKET_SIZE, MongoDbConst.REQUEST_SIZE / conditions.Count);

        var basef = Builders<Entity>.Filter
            .NearSphere(p => p.position, GeoJson.Point(new GeoJson2DGeographicCoordinates(center.lon, center.lat)), maxDistance: radius);

        return await PlaceFinder.Fetch(database, basef, conditions, limit);
    }
}
