using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GeoJSON.Text.Geometry;
using GrainPath.Application.Entities;
using GrainPath.RoutingEngine.Osrm.Helpers;

namespace GrainPath.RoutingEngine.Osrm.Fetchers;

internal static class ShortestPathFetcher
{
    private sealed class Route
    {
        /// <summary>
        /// Distance of a route in <b>meters</b>.
        /// </summary>
        public double? distance { get; set; }

        /// <summary>
        /// Duration of a route in <b>seconds</b>.
        /// </summary>
        public double? duration { get; set; }

        /// <summary>
        /// Shape of a route as GeoJSON object.
        /// </summary>
        public LineString geometry { get; set; }
    }

    private sealed class Answer
    {
        public string code { get; set; }

        public string message { get; set; }

        public List<Route> routes { get; set; }
    }

    /// <summary>
    /// Request the traversal and the distance of the shortest path from an OSRM instance.
    /// <list>
    /// <item>http://project-osrm.org/docs/v5.24.0/api/#responses</item>
    /// <item>http://project-osrm.org/docs/v5.24.0/api/#route-service</item>
    /// </list>
    /// </summary>
    /// <param name="addr">base URL of the service</param>
    /// <param name="waypoints">list of WGS84 points</param>
    /// <returns>list of shortest path objects or error message</returns>
    public static async Task<(List<ShortestPathObject>, ErrorObject)> Fetch(string addr, List<WgsPoint> waypoints)
    {
        var (b, e) = await QueryExecutor
            .Execute(QueryConstructor.Route(addr, waypoints));

        if (b is null) { return (null, e is null ? null : new() { message = e }); }

        try
        {
            var ans = JsonSerializer.Deserialize<Answer>(b);

            if (ans.code != "Ok") { return (null, null); }

            var routes = ans.routes.Select(r => new ShortestPathObject()
            {
                distance = r.distance.HasValue ? r.distance.Value : 0.0,
                duration = r.duration.HasValue ? r.duration.Value : 0.0,
                polyline = r.geometry.Coordinates
                    .Select(p => new WgsPoint(p.Longitude, p.Latitude))
                    .ToList()
            }).ToList();

            return (routes, null);
        }
        catch (Exception ex) { return (null, new() { message = ex.Message }); }
    }
}
