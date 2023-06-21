using System.Collections.Generic;
using GrainPath.Application.Entities;
using GrainPath.Application.Heuristics;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Solvers;

internal sealed class RelaxedSolver : BaseSolver
{
    private RelaxedSolver() { }

    /// <summary>
    /// IfHeuristic may return routes with repeating indices due to disjoint
    /// sets. Those can be safely removed.
    /// </summary>
    private static (List<int>, SortedSet<int>) SimplifyIfRoute(List<int> ifRoute)
    {
        var route = new List<int>();
        var occur = new SortedSet<int>();

        foreach (var index in ifRoute)
        {
            if (occur.Add(index)) { route.Add(index); }
        }

        return (route, occur);
    }

    public static List<List<int>> Solve(
        IReadOnlyList<Place> places, IDistanceMatrix matrix, double maxDistance, int routesCount)
    {
        var routes = new List<List<int>>();
        var solverPlaces = PlaceConverter.Convert(places);

        for (int i = 0; i < routesCount; ++i)
        {
            var ifRoute = IfHeuristic.Advise(solverPlaces, matrix, maxDistance, places.Count);

            if (ifRoute.Count < 3) { break; } // no more good places remained

            var (route, occur) = SimplifyIfRoute(ifRoute);

            routes.Add(TwoOptHeuristic.Refine(route, matrix));
            solverPlaces = FilterPlaces(solverPlaces, occur);
        }

        return routes;
    }
}
