using System;
using System.Collections.Generic;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Solvers;

internal sealed class RandomSolver : ISolver
{
    public List<FilteredPlace> Decide(List<FilteredPlace> places, double distance, List<KeywordCondition> conditions, List<List<double>> matrix)
    {
        var idx = 0;
        var cnt = 10;
        var rnd = new Random();

        var res = new List<FilteredPlace>();

        while (idx < places.Count && res.Count < cnt)
        {
            idx = rnd.Next(idx, places.Count);
            res.Add(places[idx]);
            ++idx;
        }

        return res;
    }
}
