using System.Collections.Generic;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Heuristics;

internal class SolverPlace
{
    public readonly int Index;
    public readonly int Category;
    public SolverPlace(int index, int category) { Index = index; Category = category; }
}

internal static class PlaceConverter
{
    public static List<SolverPlace> Convert(IReadOnlyList<Place> places)
    {
        var result = new List<SolverPlace>();

        for (int i = 0; i < places.Count; ++i)
        {
            foreach (var c in places[i].categories) { result.Add(new(i, c)); }
        }

        return result;
    }
}
