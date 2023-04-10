using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Helpers.Geometry;
using GrainPath.Application.Interfaces;
using GrainPath.Domain;
using GrainPath.Domain.Entities;

namespace GrainPath.Application.Handlers;

public static class RouteFinder
{
    /// <summary>
    /// Construct properly ordered continuous sequence.
    /// </summary>
    private static List<T> Concat<T>(T source, IEnumerable<T> waypoints, T target)
    {
        var r = new List<T>();
        r.Add(source); r.AddRange(waypoints); r.Add(target);
        return r;
    }

    /// <summary>
    /// Convert list of filtered places into a list of indexed keywords
    /// starting from index 1, because 0 is occupied by the source.
    /// </summary>
    private static List<Poi> GetPois(List<Place> places)
    {
        var pois = new List<Poi>();

        for (int i = 0; i < places.Count; ++i)
        {
            foreach (var sat in places[i].selected) { pois.Add(new Poi(i + 1, sat)); }
        }

        return pois;
    }

    /// <summary>
    /// Extract waypoints out of the route sequence. Skip first and last items,
    /// (source and target).
    /// </summary>
    private static List<Place> GetWaypoints(Route route, List<Place> filters)
    {
        var waypoints = new List<Place>();

        for (int i = 1; i < route.Sequence.Length - 1; ++i)
        {
            waypoints.Add(filters[route.Sequence[i]]);
        }

        return waypoints;
    }

    /// <summary>
    /// Calculate a route that visit places satisfying provided conditions.
    /// </summary>
    /// <param name="distance">Maximum walking distance in meters.</param>
    /// <returns></returns>
    public static async Task<(List<RouteObject>, ErrorObject)> Find(IModel model, IRoutingEngine engine, WgsPoint source, WgsPoint target, double distance, List<KeywordCondition> conditions)
    {
        // find feasible places

        var ellipse = Spherical.BoundingEllipse(source, target, distance);
        var selects = await model.GetNearestWithin(ellipse, Spherical.Midpoint(source, target), distance / 2.0, conditions);

        var locs = Concat(source, selects.Select(place => place.location), target);

        // construct distance matrix

        var (m, err1) = await engine.GetDistanceMatrix(locs);

        if (err1 is not null) { return (null, err1); }

        if (m is null) { return (null, null); }

        var matrix = new DistanceMatrix(m.distances);

        // construct routes

        var pois = Concat(new Poi(0, string.Empty), GetPois(selects), new Poi(matrix.Dim - 1, string.Empty));

        var routes = SolverFactory
            .GetInstance()
            .Solve(conditions.Select(c => c.keyword).ToList(), pois, matrix, distance);

        // construct polylines

        var polylines = new List<ShortestPathObject>();

        for (var i = 0; i < routes.Count; ++i)
        {
            var (path, err2) = await engine.GetShortestPath(routes[i].Sequence.Select(w => locs[w]).ToList());

            if (err2 is not null) { return (null, err2); }

            if (path is null) { return (null, null); }

            polylines.Add(path);
        }

        var objs = new List<RouteObject>();

        for (var i = 0; i < routes.Count; ++i)
        {
            objs.Add(new() { path = polylines[i], waypoints = GetWaypoints(routes[i], selects) });
        }

        return (objs, null);
    }
}
