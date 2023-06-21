using System.Collections.Generic;
using GrainPath.Application.Entities;
using GrainPath.Application.Heuristics;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Solvers;

internal static class PrecedenceSolver
{
    public static List<List<int>> Solve(
        IReadOnlyList<Place> places, IDistanceMatrix matrix, List<PrecedenceEdge> precedence, double maxDistance, int routesCount)
    {
        var routes = new List<List<int>>();
        var solverPlaces = PlaceConverter.Convert(places);

        for (int i = 0; i < routesCount; ++i)
        {
            var ogRoute = OgHeuristic.Advise(solverPlaces, matrix, precedence, maxDistance, places.Count);

            if (ogRoute.Count < 3) { break; }

            routes.Add(ogRoute);
        }

        return routes;
    }
}
