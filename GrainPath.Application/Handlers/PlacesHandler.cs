using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Handlers;

/// <summary>
/// <c>/places</c> request handler.
/// </summary>
public static class PlacesHandler
{
    private static readonly int TOTAL_SIZE = 100;
    private static readonly int MIN_BUCKET_SIZE = 25;

    /// <summary>
    /// Get a list of matching places. The result is capped to avoid retrieving
    /// the entire database.
    /// </summary>
    public static Task<(List<Place>, ErrorObject)> Handle(
        IModel model, WgsPoint center, double radius, List<Category> categories)
    {
        var bucket = Math.Max(MIN_BUCKET_SIZE, TOTAL_SIZE / categories.Count);

        return model.GetAround(center, radius, categories, bucket);
    }
}
