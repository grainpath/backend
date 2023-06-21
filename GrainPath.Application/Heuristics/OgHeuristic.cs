using System.Collections.Generic;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Heuristics;

internal sealed class OgCategory
{
    public int pred = 0;
    public readonly SortedSet<int> succ = new();
    public readonly List<SolverPlace> places = new();
}

internal static class OgCategoryFormer
{
    public static SortedDictionary<int, OgCategory> Form(IReadOnlyList<SolverPlace> places, List<PrecedenceEdge> precedence)
    {
        var result = new SortedDictionary<int, OgCategory>();

        var ensureKey = (SortedDictionary<int, OgCategory> dict, int key) =>
        {
            if (!dict.ContainsKey(key)) { dict.Add(key, new()); }
        };

        foreach (var place in places)
        {
            ensureKey(result, place.Category);
            result[place.Category].places.Add(place);
        }

        foreach (var prec in precedence)
        {
            ensureKey(result, prec.fr);
            ensureKey(result, prec.to);

            if (result[prec.fr].succ.Add(prec.to)) { ++result[prec.to].pred; }
        }

        return result;
    }
}

/// <summary>
/// Oriented Greedy Heuristic from https://doi.org/10.14778/1920841.1920861.
/// </summary>
internal static class OgHeuristic
{
    public static List<int> Advise(
        IReadOnlyList<SolverPlace> places, IDistanceMatrix matrix, List<PrecedenceEdge> precedence, double maxDistance, int placesCount)
    {
        var seq = new List<int>() { 0, placesCount - 1 };
        var distance = matrix.Distance(0, placesCount - 1);

        var cats = OgCategoryFormer.Form(places, precedence);

        while (cats.Count > 0)
        {
            // TODO: (!)
        }

        return seq;
    }
}
