using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Entities;

namespace GrainPath.Api.Helpers;

internal static class CategoryVerifier
{
    public static bool Verify(Category category)
    {
        var err = false;
        var nums = category.filters.numerics;

        foreach (var n in new[] { nums.year, nums.rating, nums.capacity, nums.elevation, nums.minimumAge })
        {
            err |= n is not null && n.max < n.min;
        }

        return !err;
    }

    public static bool Verify(List<Category> categories)
    {
        return categories.Aggregate(true, (acc, category) => acc && Verify(category));
    }
}
