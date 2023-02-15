using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using backend.Entity;
using backend.Utility;
using GeoJSON.Text.Geometry;

namespace backend.RoutingEngine;

static class OsrmShortestPathFinder
{
    private static readonly string _prefix = @"route/v1/foot/";
    private static readonly string _suffix = @"?geometries=geojson&skip_waypoints=true";

    private sealed class Status
    {
        public string code { get; set; }

        public string message { get; set; }
    }

    private sealed class Route
    {
        public double? distance { get; set; }

        public LineString geometry { get; set; }
    }

    private sealed class Answer
    {
        public string code { get; set; }

        public List<Route> routes { get; set; }
    }

    private static string status(string body)
    {
        var stat = JsonSerializer.Deserialize<Status>(body);

        if (stat.code != "Ok") { throw new Exception(); }

        return body;
    }

    private static Answer answer(string body)
    {
        var ans = JsonSerializer.Deserialize<Answer>(body);

        if (ans.routes is null || ans.routes.Count == 0) { throw new Exception(); }

        return ans;
    }

    public static async Task<ShortestPath> Find(string addr, List<WebPoint> sequence)
    {
        var seq = sequence.Select(p => p.lon.ToString() + ',' + p.lat.ToString());
        var qry = addr + _prefix + string.Join(';', seq) + _suffix;

        var ans = await HttpCaller.Body(qry)
            .ContinueWith(body => status(body.Result))
            .ContinueWith(body => answer(body.Result));

        var route = ans.routes.First();

        if (route.geometry is null || route.distance is null) { throw new Exception(); }

        return new()
        {
            distance = route.distance.Value,
            route = route.geometry.Coordinates
                .Select(p => new WebPoint() { lon = p.Longitude, lat = p.Latitude })
                .ToList()
        };
    }
}
