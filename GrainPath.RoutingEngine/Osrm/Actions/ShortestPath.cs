using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GeoJSON.Text.Geometry;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.RoutingEngine.Osrm.Actions;

internal static class ShortestPath
{
    private sealed class Route
    {
        public double? distance { get; set; }

        public double? duration { get; set; }

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

    public static async Task<ShortObject> Act(string addr, List<WebPoint> sequence)
    {
        var sview = sequence.Select(p => p.lon.ToString() + ',' + p.lat.ToString());
        var query = addr + _prefix + string.Join(';', sview) + _suffix;

        var report = (HttpStatusCode code) => $"Osrm server answered with status code ${code}.";

        /**
         * Osrm http request could return 200 or 400. We consider other status
         * codes as an evidence that the server is temporarily non-operable.
         * See http://project-osrm.org/docs/v5.24.0/api/#responses
         */

        // http request

        HttpResponseMessage res;

        try {
            res = await new HttpClient().GetAsync(query);
        } catch (Exception ex) { return new() { status = RoutingEngineStatus.UN, message = ex.Message }; }

        if (res.StatusCode == HttpStatusCode.BadRequest) {
            return new() { status = RoutingEngineStatus.BR, message = report(res.StatusCode) };
        }

        if (res.StatusCode != HttpStatusCode.OK) {
            return new() { status = RoutingEngineStatus.UN, message = report(res.StatusCode) };
        }

        var body = await res.Content.ReadAsStringAsync();

        // deserialize body

        Answer ans;

        try {
            ans = JsonSerializer.Deserialize<Answer>(body);
        } catch (Exception ex) { return new() { status = RoutingEngineStatus.BR, message = ex.Message }; }

        if (ans.code != "Ok" || ans.routes is null || ans.routes.Count == 0) {
            return new() { status = RoutingEngineStatus.BR, message = ans.message };
        }

        var route = ans.routes.First();

        if (route.distance is null || route.duration is null || route.geometry is null) {
            return new() { status = RoutingEngineStatus.BR, message = "Empty distance, or duration, or geometry." };
        }

        // construct object

        return new()
        {
            status = RoutingEngineStatus.OK,
            response = new()
            {
                distance = route.distance.Value,
                duration = route.duration.Value,
                polyline = route.geometry.Coordinates
                    .Select(p => new WebPoint() { lon = p.Longitude, lat = p.Latitude })
                    .ToList()
            }
        };
    }
}
