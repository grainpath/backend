using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace GrainPath.Data.MongoDb.Actions;

internal static class Stack
{
    private class StackItemComparer : IEqualityComparer<FilteredPlace>
    {
        public bool Equals(FilteredPlace l, FilteredPlace r) => l.place.id == r.place.id;

        public int GetHashCode([DisallowNull] FilteredPlace obj) => throw new NotImplementedException();
    }

    public static async Task<List<FilteredPlace>> Act(IMongoDatabase database, StackRequest request)
    {
        var l = Math.Max(MongoDbConst.BUCKET_SIZE, MongoDbConst.REQUEST_SIZE / request.conditions.Count);

        var b = Builders<HeavyPlace>.Filter
            .Near(p => p.position, GeoJson.Point(GeoJson.Position(request.center.lon.Value, request.center.lat.Value)), maxDistance: request.radius * 1000.0);

        var r = new HashSet<FilteredPlace>();

        foreach (var cond in request.conditions) {

            var places = await database
                .GetCollection<HeavyPlace>(MongoDbConst.GRAIN_COLLECTION)
                .Find(b & FilterConstructor.ConditionToFilter(cond))
                .Limit(l)
                .ToListAsync();

            var items = places.Select(p => new FilteredPlace() {
                place = new() { id = p.id, name = p.name, location = p.location, keywords = p.keywords },
                satisfy = new() { cond.keyword }
            });

            foreach (var item in items) {
                if (r.TryGetValue(item, out var val)) {
                    val.satisfy.Add(cond.keyword);
                }
                else { r.Add(item); }
            }
        }

        return r.ToList();
    }
}
