using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using GrainPath.Domain.Entities;
using GrainPath.Domain.Heuristics;
using GrainPath.Domain.Interfaces;

internal sealed class StandardSolver : ISolver
{
    public List<Route> Solve(IReadOnlyList<string> keywords, IReadOnlyList<Poi> pois, DistanceMatrix matrix, double distance)
    {
        var routes = IfHeuristic
            .Advise(keywords, pois, matrix, distance);

        for (int i = 0; i < routes.Count; ++i) {
            routes[i].Sequence = TwoOptHeuristic
                .Advise(routes[i].Sequence.ToList(), routes[i].Matrix)
                .ToImmutableArray();
        }

        return routes;
    }
}
