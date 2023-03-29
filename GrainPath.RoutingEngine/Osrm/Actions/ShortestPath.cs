using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GeoJSON.Text.Geometry;
using GrainPath.Application.Entities;

namespace GrainPath.RoutingEngine.Osrm.Actions;

internal static class ShortestPath
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

    private static readonly string _prefix = @"/route/v1/foot/";
    private static readonly string _suffix = @"?geometries=geojson&skip_waypoints=true";

    public static async Task<(ShortestPathObject, ErrorObject)> Act(string addr, List<WebPoint> sequence)
    {
        var sview = sequence.Select(p => p.lon.ToString() + ',' + p.lat.ToString());
        var query = addr + _prefix + string.Join(';', sview) + _suffix;

        /**
         * Osrm http request could return 200 or 400. We consider other status
         * codes as an evidence that the server is temporarily non-operable.
         * See http://project-osrm.org/docs/v5.24.0/api/#responses
         */

        // http request

        var (b, e) = await RoutingEngineFetcher.GetBody(query);

        if (e is not null) { return (null, new() { message = e }); }

        if (b is null) { return (null, null); }

        // assume well-formed answer object

        try {
            var ans = JsonSerializer.Deserialize<Answer>(b);

            if (ans.code != "Ok" || ans.routes.Count == 0) { return (null, null); }

            var route = ans.routes.First();

            return (new() {
                distance = route.distance.Value,
                duration = route.duration.Value,
                polyline = route.geometry.Coordinates
                    .Select(p => new WebPoint() { lon = p.Longitude, lat = p.Latitude })
                    .ToList()
            }, null);
        }
        catch (Exception ex) { return (null, new() { message = ex.Message }); }
    }
}
