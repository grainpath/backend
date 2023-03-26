using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace GrainPath.Data.MongoDb.Actions;

using B = FilterDefinitionBuilder<PositionedHeavyPlace>;
using F = FilterDefinition<PositionedHeavyPlace>;
using EB = Expression<Func<PositionedHeavyPlace, bool?>>;
using EC = Expression<Func<PositionedHeavyPlace, IEnumerable<string>>>;
using EN = Expression<Func<PositionedHeavyPlace, int?>>;

internal static class FilterDefinitionExtensions
{
    public static F boolean(this F f, B b, bool? x, EB e)
    {
        return (x is null)
            ? f
            : f & b.Eq(e, x);
    }

    public static F numeric(this F f, B b, KeywordNumericFilter x, EN e)
    {
        return (x is null)
            ? f
            : f & b.Gte(e, x.min) & b.Lte(e, x.max);
    }

    public static F collect(this F f, B b, KeywordCollectFilter x, EC e)
    {
        f = (x is null || x.includes.Count == 0)
            ? f
            : f & b.AnyIn(e, x.includes);

        f = (x is null || x.excludes.Count == 0)
            ? f
            : f & b.Not(b.AnyIn(e, x.excludes));

        return f;
    }
}

internal static class Stack
{
    ///<summary>
    /// Construct database-specific filter out of the provided keyword condition.
    ///</summary>
    private static F getFilter(KeywordCondition condition) {

        var filters = condition.filters;
        var b = Builders<PositionedHeavyPlace>.Filter;

        var f = b.Empty;

        // keywords

        f = f & b.AnyEq(o => o.keywords, condition.keyword);

        // existens

        f = (filters.image is null)
            ? f
            : f & b.Exists(o => o.features.image);

        f = (filters.description is null)
            ? f
            : f & b.Exists(o => o.features.description);

        f = (filters.website is null)
            ? f
            : f & b.Exists(o => o.features.website);

        f = (filters.address is null)
            ? f
            : f & b.Exists(o => o.features.address);

        f = (filters.payment is null)
            ? f
            : f & b.Exists(o => o.features.payment);

        f = (filters.email is null)
            ? f
            : f & b.Exists(o => o.features.email);

        f = (filters.phone is null)
            ? f
            : f & b.Exists(o => o.features.phone);

        f = (filters.charge is null)
            ? f
            : f & b.Exists(o => o.features.charge);

        f = (filters.opening_hours is null)
            ? f
            : f & b.Exists(o => o.features.opening_hours);

        f = f
            .boolean(b, filters.fee, p => p.features.fee)
            .boolean(b, filters.delivery, p => p.features.delivery)
            .boolean(b, filters.drinking_water, p => p.features.drinking_water)
            .boolean(b, filters.internet_access, p => p.features.internet_access)
            .boolean(b, filters.shower, p => p.features.shower)
            .boolean(b, filters.smoking, p => p.features.smoking)
            .boolean(b, filters.takeaway, p => p.features.takeaway)
            .boolean(b, filters.toilets, p => p.features.toilets)
            .boolean(b, filters.wheelchair, p => p.features.wheelchair);

        f = f
            .numeric(b, filters.rank, p => p.features.rank)
            .numeric(b, filters.capacity, p => p.features.capacity)
            .numeric(b, filters.minimum_age, p => p.features.minimum_age);

        f = (filters.name is null)
            ? f
            : f & b.StringIn(o => o.features.name, new BsonRegularExpression(filters.name, "i"));

        f = f
            .collect(b, filters.rental, p => p.features.rental)
            .collect(b, filters.clothes, p => p.features.clothes)
            .collect(b, filters.cuisine, p => p.features.cuisine);

        return f;
    }

    private class StackItemComparer : IEqualityComparer<StackItem>
    {
        public bool Equals(StackItem l, StackItem r) => l.place.id == r.place.id;

        public int GetHashCode([DisallowNull] StackItem obj) => throw new NotImplementedException();
    }

    public static async Task<List<StackItem>> Act(IMongoDatabase database, StackRequest request)
    {
        var l = Math.Max(20, 100 / request.conditions.Count);

        var b = Builders<PositionedHeavyPlace>.Filter
            .Near(p => p.position, GeoJson.Point(GeoJson.Position(request.center.lon.Value, request.center.lat.Value)), maxDistance: request.radius * 1000.0);

        var r = new HashSet<StackItem>();

        foreach (var cond in request.conditions) {

            var places = await database
                .GetCollection<PositionedHeavyPlace>(MongoDbConst.GrainCollection)
                .Find(b & getFilter(cond))
                .Limit(l)
                .ToListAsync();

            var items = places.Select(p => new StackItem() {
                place = new() { id = p.id, name = p.name, location = p.location, keywords = p.keywords },
                fulfill = new() { cond.keyword }
            });

            foreach (var item in items) {
                if (r.TryGetValue(item, out var val)) {
                    val.fulfill.Add(cond.keyword);
                }
                else { r.Add(item); }
            }
        }

        return r.ToList();
    }
}
