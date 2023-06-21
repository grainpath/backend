using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Entities;
using GrainPath.Application.Heuristics;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Solvers;

internal static class RelaxedSolver
{
    /// <summary>
    /// IfHeuristic may return routes with repeating indices due to disjoint
    /// sets. Those can be safely removed.
    /// </summary>
    private static (List<int>, SortedSet<int>) SimplifyIfRoute(List<int> ifRoute)
    {
        var route = new List<int>();
        var dict = new SortedSet<int>();

        foreach (var point in ifRoute)
        {
            if (!dict.Contains(point)) { route.Add(point); dict.Add(point); }
        }

        return (route, dict);
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

            var (route, dict) = SimplifyIfRoute(ifRoute);

            routes.Add(TwoOptHeuristic.Advise(route, matrix));
            solverPlaces = solverPlaces.Where(ifPlace => !dict.Contains(ifPlace.Index)).ToList();
        }

        return routes;
    }
}
