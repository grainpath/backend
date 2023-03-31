using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Helpers.Geometry;
using GrainPath.Application.Interfaces;
using GrainPath.Application.Solvers;

namespace GrainPath.Application.Handlers;

public static class RouteFinder
{
    /// <summary>
    /// Construct <b>ordered</b> sequence of waypoints.
    /// </summary>
    private static List<WgsPoint> GetWaypoints(WgsPoint source, WgsPoint target, IEnumerable<WgsPoint> places)
    {
        var waypoints = new List<WgsPoint>() { source };
        waypoints.AddRange(places);
        waypoints.Add(target);

        return waypoints;
    }

    /// <summary>
    /// Calculate a route that visit places satisfying provided conditions.
    /// </summary>
    /// <param name="distance">Maximum walking distance in meters.</param>
    /// <returns></returns>
    public static async Task<(RouteObject, ErrorObject)> Find(IModel model, IRoutingEngine engine, WgsPoint source, WgsPoint target, double distance, List<KeywordCondition> conditions)
    {
        // find feasible places

        var ellipse = Spherical.BoundingEllipse(source, target, distance);
        var places = await model.GetWithin(ellipse, conditions);

        // distance matrix

        var (mat, err1) = await engine.GetDistanceMatrix(GetWaypoints(source, target, places.Select(p => p.place.location)));

        if (err1 is not null) { return (null, err1); }

        if (mat is null) { return (null, null); }

        // construct order

        var order = SolverFactory
            .GetInstance()
            .Decide(places, distance, conditions, mat.distances);

        // construct polyline

        var (path, err2) = await engine.GetShortestPath(GetWaypoints(source, target, order.Select(p => p.place.location)));

        if (err2 is not null) { return (null, err2); }

        if (path is null) { return (null, null); }

        // success

        return (new() { path = path, order = order }, null);
    }
}
