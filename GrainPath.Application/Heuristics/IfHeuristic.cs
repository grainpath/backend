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

internal sealed class IfCategoryComparer : IComparer<List<IfPlace>>
{
    /// <summary>
    /// Categories with less items are more relevant.
    /// </summary>
    public int Compare(List<IfPlace> l, List<IfPlace> r) => l.Count.CompareTo(r.Count);
}

internal static class IfPlaceSeparator
{
    /// <summary>
    /// Separate points by category.
    /// </summary>
    private static List<List<IfPlace>> Group(IReadOnlyList<IfPlace> places)
    {
        return places.Aggregate(new SortedDictionary<int, List<IfPlace>>(), (acc, place) =>
        {
            if (!acc.ContainsKey(place.Category))
            {
                acc.Add(place.Category, new List<IfPlace>());
            }
            acc[place.Category].Add(place);
            return acc;
        }).Values.ToList();
    }

    /// <summary>
    /// Sort categories by number of elements in ascending order.
    /// </summary>
    private static List<List<IfPlace>> Sort(List<List<IfPlace>> categories)
    {
        categories.Sort(new IfCategoryComparer());
        return categories;
    }

    /// <summary>
    /// Group places by category and sort categories by relevancy.
    /// </summary>
    public static List<List<IfPlace>> Separate(IReadOnlyList<IfPlace> places) => Sort(Group(places));
}

internal static class IfCandidateFinder
{
    /// <summary>
    /// Given a certain keyword, find a pair of poi and position for insertion
    /// that gives the smallest distance increase.
    /// </summary>
    public static (IfPlace, int, double) FindBest(
        IReadOnlyList<int> seq, IReadOnlyList<IfPlace> cat, IDistanceMatrix matrix, double currDistance)
    {
        int index = -1;
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
                    index = i;
                    best = place;
                    candDistance = nextDistance;
                }
            }
        }

        return (best, index, candDistance);
    }
}

/// <summary>
/// Infrequent-First Heuristic from https://doi.org/10.1145/1463434.1463449.
/// </summary>
internal static class IfHeuristic
{
    /// <summary>
    /// Advise a route.
    /// </summary>
    public static List<int> Advise(
        IReadOnlyList<IfPlace> pois, IDistanceMatrix matrix, double maxDistance, int placeCount)
    {
        var seq = new List<int>() { 0, placeCount - 1 };
        var distance = matrix.Distance(0, placeCount - 1);

        var cats = IfPlaceSeparator.Separate(pois);

        foreach (var cat in cats)
        {
            var (best, seqIndex, candDistance) = IfCandidateFinder.FindBest(seq, cat, matrix, distance);

            if (best is not null && candDistance <= maxDistance * 1.0)
            {
                distance = candDistance;
                seq.Insert(seqIndex, best.Index);
            }
        }

        return seq;
    }
}
