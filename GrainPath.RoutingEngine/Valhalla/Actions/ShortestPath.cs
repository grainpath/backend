using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.RoutingEngine.Valhalla.Actions;

internal static class ShortestPath
{
    private sealed class Query
    {
        public string units => "kilometers";

        public string costing => "pedestrian";

        public List<WebPoint> locations { get; set; }
    }

    private sealed class Leg
    {
        public string shape { get; set; }
    }

    private sealed class Summary
    {
        /// <summary>
        /// Distance of a route in <b>specified units (kilometers)</b>.
        /// </summary>
        public double? length { get; set; }

        /// <summary>
        /// Duration of a route in <b>seconds</b>.
        /// </summary>
        public double? time { get; set; }
    }

    private sealed class Trip
    {
        public long? status { get; set; }

        public string status_message { get; set; }

        public Summary summary { get; set; }

        public List<Leg> legs { get; set; }
    }

    private sealed class Answer
    {
        public Trip trip { get; set; }
    }

    /// <summary>
    /// Based on <see href="https://valhalla.github.io/valhalla/decoding/"/>.
    /// </summary>
    private static List<WebPoint> decode(string polyline)
    {
        int i = 0;
        double precision = 1.0 / 1e6;

        var deserialize = (int prev) =>
        {
            int b, shift = 0, result = 0;
            do {

                b = polyline[i++] - 63;
                result |= (b & 0x1f) << shift;
                shift += 5;

            } while (b >= 0x20);

            return prev + ((result & 1) == 1 ? ~(result >> 1) : result >> 1);
        };

        var points = new List<WebPoint>();
        int lastLon = 0, lastLat = 0;

        while (i < polyline.Length) {

            int lon = deserialize(lastLon);
            int lat = deserialize(lastLat);

            points.Add(new() { lon = lon * precision, lat = lat * precision });
            lastLon = lon;
            lastLat = lat;
        }

        return points;
    }

    private static string _prefix = "/route?json=";

    public static async Task<(ShortestPathObject, ErrorObject)> Act(string addr, List<WebPoint> sequence)
    {
        var suffix = new Query() { locations = sequence };
        var query = addr + _prefix + JsonSerializer.Serialize(suffix);

        /**
         * Valhalla turn-by-turn API follows the Http specification, see documentation at
         * https://valhalla.github.io/valhalla/api/turn-by-turn/api-reference/#http-status-codes-and-conditions
         */

        // http request

        var (b, e) = await RoutingEngineFetcher.GetBody(query);

        if (e is not null) { return (null, new() { message = e }); }

        if (b is null) { return (null, null); }

        // assume well-formed answer object

        try {
            var ans = JsonSerializer.Deserialize<Answer>(b);

            var shape = new List<WebPoint>();

            for (int i = 0; i < ans.trip.legs.Count; ++ i) {

                var partial = ans.trip.legs[i].shape;

                foreach (var point in decode(partial)) {
                    if (shape.Count == 0 || point.lon != shape[^1].lon || point.lat != shape[^1].lat) {
                        shape.Add(point);
                    }
                }
            }

            return (new() {
                distance = ans.trip.summary.length.Value * 1000,
                duration = ans.trip.summary.time.Value,
                polyline = shape
            }, null);
        }
        catch (Exception ex) { return (null, new() { message = ex.Message }); }
    }
}
