using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Heuristics;

internal sealed class IfPlace
{
    public readonly int Index;
    public readonly int Category;
    public IfPlace(int index, int category) { Index = index; Category = category; }
}

/// <summary>
/// Infrequent-First Heuristic from https://doi.org/10.1145/1463434.1463449.
/// </summary>
internal static class IfHeuristic
{
    /// <summary>
    /// Categories with less items are more relevant.
    /// </summary>
    private sealed class CategoryComparer : IComparer<List<IfPlace>>
    {
        public int Compare(List<IfPlace> l, List<IfPlace> r) => l.Count.CompareTo(r.Count);
    }

    /// <summary>
    /// Separate points by category.
    /// </summary>
    private static List<List<IfPlace>> GetCategories(IReadOnlyList<IfPlace> places, int catsCount)
    {
        var categories = Enumerable.Range(0, catsCount).Select(_ => new List<IfPlace>()).ToList();

        foreach (var place in places) { categories[place.Category].Add(place); }

        return categories;
    }

    /// <summary>
    /// Sort categories by number of elements in ascending order.
    /// </summary>
    private static List<List<IfPlace>> SortCategories(List<List<IfPlace>> categories)
    {
        categories.Sort(new CategoryComparer());
        return categories;
    }

    /// <summary>
    /// Given a certain keyword, find a pair of poi and position for insertion
    /// that gives the smallest distance increase.
    /// </summary>
    private static (IfPlace, int, double) FindBest(IReadOnlyList<int> seq, IReadOnlyList<IfPlace> cat, IDistanceMatrix matrix, double currDistance)
    {
        int seqIndex = -1;
        IfPlace best = null;
        double candDistance = double.MaxValue;

        foreach (var place in cat)
        {
            for (int i = 1; i < seq.Count; ++i)
            {
                var nextDistance = currDistance
                    - matrix.Distance(seq[i - 1], seq[i])
                    + matrix.Distance(seq[i - 1], place.Index)
                    + matrix.Distance(place.Index, seq[i]);

                if (nextDistance < candDistance)
                {
                    best = place;
                    seqIndex = i;
                    candDistance = nextDistance;
                }
            }
        }

        return (best, seqIndex, candDistance);
    }

    /// <summary>
    /// Advise a route.
    /// </summary>
    public static List<int> Advise(
        IReadOnlyList<IfPlace> pois, IDistanceMatrix matrix, double maxDistance, int catsCount, int placeCount)
    {
        var seq = new List<int>() { 0, placeCount - 1 };
        var distance = matrix.Distance(0, placeCount - 1);

        var cats = SortCategories(GetCategories(pois, catsCount));

        foreach (var cat in cats)
        {
            var (best, seqIndex, candDistance) = FindBest(seq, cat, matrix, distance);

            if (best is not null && candDistance <= maxDistance * 1.0)
            {
                distance = candDistance;
                seq.Insert(seqIndex, best.Index);
            }
        }

        return seq;
    }
}
