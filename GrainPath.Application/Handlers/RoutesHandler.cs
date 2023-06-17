using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Algorithms;
using GrainPath.Application.Interfaces;
using GrainPath.Application.Solvers;

namespace GrainPath.Application.Handlers;

public static class RoutesHandler
{
    /// <summary>
    /// Consider at most quantity places in each category.
    /// </summary>
    private static readonly int BUCKET_SIZE = 20;

    /// <summary>
    /// Selector between Osrm (precise) and Haversine (approximate) distance matrices.
    /// </summary>
    private static readonly int DISTANCE_MATRIX_THRESHOLD = 5;

    /// <summary>
    /// Construct properly ordered continuous sequence.
    /// </summary>
    private static List<T> Concat<T>(T source, IEnumerable<T> waypoints, T target)
        => new List<T>().Concat(new[] { source }).Concat(waypoints).Concat(new[] { target }).ToList();

    /// <summary>
    /// Extract waypoints out of the route sequence. Skip first and last items
    /// (source and target).
    /// </summary>
    private static List<Place> ExtractWaypoints(List<int> route, List<Place> places)
    {
        return Enumerable.Range(1, route.Count - 1)
            .Aggregate(new List<Place>(), (acc, i) => { acc.Add(places[route[i]]); return acc; });
    }

    /// <summary>
    /// Calculate a route that visit places of provided categories.
    /// </summary>
    public static async Task<(List<RouteObject>, ErrorObject)> Handle(
        IModel model, IRoutingEngine engine, WgsPoint source, WgsPoint target,
        double distance, List<Category> categories, List<PrecedenceEdge> precedence)
    {
        /* Get list of places and locations within bounding ellipse. Note that
         * the source and target are part of the list. */

        var ellipse = Spherical.BoundingEllipse(source, target, distance);

        var (around, err0) = await model.GetAroundWithin(
            ellipse, Spherical.Midpoint(source, target), distance / 2.0, categories, BUCKET_SIZE);

        if (around is null) { return (new(), err0); }

        var places = Concat(new Place() { location = source }, around, new Place() { location = target });
        var locations = places.Select(place => place.location).ToList();

        // Obtain distance matrix.

        var (matrix, err1) = categories.Count <= DISTANCE_MATRIX_THRESHOLD
            ? (await engine.GetDistanceMatrix(locations))
            : (new HaversineDistanceMatrix(locations), null);

        if (matrix is null) { return (new(), err1); }

        // Construct waypoint sequences.

        var routes = (precedence.Count == 0)
            ? (RelaxedSolver.Solve(places, matrix, categories.Count, distance))
            : (PrecedenceSolver.Solve(places, matrix, categories.Count, distance, precedence));

        // Construct polylines.

        var polylines = new List<ShortestPathObject>();

        for (var i = 0; i < routes.Count; ++i)
        {
            var (path, err2) = await engine.GetShortestPath(routes[i].Select(w => locations[w]).ToList());

            if (path.Count == 0) { return (new(), err2); }

            polylines.Add(path[0]);
        }

        // Finalize route objects.

        var objs = Enumerable.Range(0, routes.Count).Aggregate(new List<RouteObject>(), (acc, i) => {
            acc.Add(new() { path = polylines[i], waypoints = ExtractWaypoints(routes[i], places) }); return acc; });

        return (objs, null);
    }
}
