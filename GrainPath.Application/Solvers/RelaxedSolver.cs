using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Entities;
using GrainPath.Application.Heuristics;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Solvers;

internal static class RelaxedSolver
{
    /// <summary>
    /// Get a suitable input to IfHeuristic.
    /// </summary>
    private static List<IfPlace> GetIfPlaces(IReadOnlyList<Place> places)
    {
        var ifPlaces = new List<IfPlace>();

        for (int i = 0; i < places.Count; ++i)
        {
            foreach (var c in places[i].categories) { ifPlaces.Add(new(i, c)); }
        }

        return ifPlaces;
    }

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
        IReadOnlyList<Place> places, IDistanceMatrix matrix, double distance, int catsCount, int routesCount)
    {
        var routes = new List<List<int>>();
        var ifPlaces = GetIfPlaces(places);

        for (int i = 0; i < routesCount; ++i)
        {
            var ifRoute = IfHeuristic.Advise(ifPlaces, matrix, distance, places.Count);

            if (ifRoute.Count <= 2) { break; } // no more good candidates remained

            var (route, dict) = SimplifyIfRoute(ifRoute);

            routes.Add(TwoOptHeuristic.Advise(route, matrix));
            ifPlaces = ifPlaces.Where(ifPlace => !dict.Contains(ifPlace.Index)).ToList();
        }

        return routes;
    }
}
