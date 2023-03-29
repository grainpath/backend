using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using GrainPath.Application.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb;

using B = FilterDefinitionBuilder<HeavyPlace>;
using F = FilterDefinition<HeavyPlace>;
using EE = Expression<Func<HeavyPlace, object>>;
using EB = Expression<Func<HeavyPlace, bool?>>;
using EC = Expression<Func<HeavyPlace, IEnumerable<string>>>;
using ET = Expression<Func<HeavyPlace, string>>;
using EN = Expression<Func<HeavyPlace, double?>>;

internal static class FilterDefinitionExtensions
{
    public static F existen(this F f, B b, object x, EE e)
        => (x is null) ? f : f & b.Exists(e);

    public static F boolean(this F f, B b, bool? x, EB e)
        => (x is null) ? f : f & b.Eq(e, x);

    public static F numeric(this F f, B b, KeywordNumericFilter x, EN e)
        => (x is null) ? f : f & b.Gte(e, x.min) & b.Lte(e, x.max);

    public static F textual(this F f, B b, string x, ET e)
        => (x is null) ? f : f & b.StringIn(e, new BsonRegularExpression(Regex.Escape(x), "i"));

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

internal static class FilterConstructor
{
    ///<summary>
    /// Construct database-specific filter out of the provided keyword condition.
    ///</summary>
    public static F ConditionToFilter(KeywordCondition condition)
    {
        var filters = condition.filters;
        var b = Builders<HeavyPlace>.Filter;

        var f = b.Empty;

        f = f & b.AnyEq(o => o.keywords, condition.keyword);

        f = f
            .existen(b, filters.image, p => p.features.image)
            .existen(b, filters.description, p => p.features.description)
            .existen(b, filters.website, p => p.features.website)
            .existen(b, filters.address, p => p.features.address)
            .existen(b, filters.payment, p => p.features.payment)
            .existen(b, filters.email, p => p.features.email)
            .existen(b, filters.phone, p => p.features.phone)
            .existen(b, filters.charge, p => p.features.charge)
            .existen(b, filters.opening_hours, p => p.features.opening_hours);

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

        f = f
            .textual(b, filters.name, p => p.features.name);

        f = f
            .collect(b, filters.rental, p => p.features.rental)
            .collect(b, filters.clothes, p => p.features.clothes)
            .collect(b, filters.cuisine, p => p.features.cuisine);

        return f;
    }
}
