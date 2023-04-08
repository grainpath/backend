using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using GrainPath.Application.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GrainPath.Data.MongoDb.Helpers;

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

    public static F numeric(this F f, B b, KeywordFilterNumeric x, EN e)
        => (x is null) ? f : f & b.Gte(e, x.min) & b.Lte(e, x.max);

    public static F textual(this F f, B b, string x, ET e)
        => (x is null) ? f : f & b.StringIn(e, new BsonRegularExpression(Regex.Escape(x), "i"));

    public static F collect(this F f, B b, KeywordFilterCollect x, EC e)
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
            .existen(b, filters.existens.image, p => p.attributes.image)
            .existen(b, filters.existens.description, p => p.attributes.description)
            .existen(b, filters.existens.website, p => p.attributes.website)
            .existen(b, filters.existens.address, p => p.attributes.address)
            .existen(b, filters.existens.payment, p => p.attributes.payment)
            .existen(b, filters.existens.email, p => p.attributes.email)
            .existen(b, filters.existens.phone, p => p.attributes.phone)
            .existen(b, filters.existens.charge, p => p.attributes.charge)
            .existen(b, filters.existens.openingHours, p => p.attributes.openingHours);

        f = f
            .boolean(b, filters.booleans.fee, p => p.attributes.fee)
            .boolean(b, filters.booleans.delivery, p => p.attributes.delivery)
            .boolean(b, filters.booleans.drinkingWater, p => p.attributes.drinkingWater)
            .boolean(b, filters.booleans.internetAccess, p => p.attributes.internetAccess)
            .boolean(b, filters.booleans.shower, p => p.attributes.shower)
            .boolean(b, filters.booleans.smoking, p => p.attributes.smoking)
            .boolean(b, filters.booleans.takeaway, p => p.attributes.takeaway)
            .boolean(b, filters.booleans.toilets, p => p.attributes.toilets)
            .boolean(b, filters.booleans.wheelchair, p => p.attributes.wheelchair);

        f = f
            .numeric(b, filters.numerics.rank, p => p.attributes.rank)
            .numeric(b, filters.numerics.capacity, p => p.attributes.capacity)
            .numeric(b, filters.numerics.minimumAge, p => p.attributes.minimumAge);

        f = f
            .textual(b, filters.textuals.name, p => p.attributes.name);

        f = f
            .collect(b, filters.collects.rental, p => p.attributes.rental)
            .collect(b, filters.collects.clothes, p => p.attributes.clothes)
            .collect(b, filters.collects.cuisine, p => p.attributes.cuisine);

        return f;
    }
}
