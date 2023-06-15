using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GeoJSON.Text.Geometry;
using GrainPath.Application.Entities;

namespace GrainPath.RoutingEngine.Osrm.Actions;

internal static class OsrmShortestPath
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
    /// <returns>shortest path object or error message</returns>
    public static async Task<(ShortestPathObject, ErrorObject)> Get(string addr, List<WgsPoint> waypoints)
    {
        var (b, e) = await OsrmFetcher
            .GetBody(OsrmQueryConstructor.Route(addr, waypoints));

        if (b is null) { return (null, e is null ? null : new() { message = e }); }

        try
        {
            var ans = JsonSerializer.Deserialize<Answer>(b);

            if (ans.code != "Ok" || ans.routes.Count == 0) { return (null, null); }

            var route = ans.routes.First();

            return (new()
            {
                distance = route.distance.HasValue ? route.distance.Value : 0.0,
                duration = route.duration.HasValue ? route.duration.Value : 0.0,
                polyline = route.geometry.Coordinates
                    .Select(p => new WgsPoint(p.Longitude, p.Latitude))
                    .ToList()
            }, null);
        }
        catch (Exception ex) { return (null, new() { message = ex.Message }); }
    }
}
