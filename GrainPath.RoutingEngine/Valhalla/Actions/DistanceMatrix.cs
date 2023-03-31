using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.RoutingEngine.Valhalla.Actions;

internal static class DistanceMatrix
{
    private sealed class Answer
    {
        internal sealed class Item
        {
            /// <summary>
            /// Distance in kilometers.
            /// </summary>
            public double distance { get; set; }
        }

        public List<List<Item>> sources_to_targets { get; set; }
    }

    /// <summary>
    /// Find shortest paths between all pairs of points.
    /// </summary>
    /// <returns>Distance matrix object in meters.</returns>
    public static async Task<(DistanceMatrixObject, ErrorObject)> Act(string addr, List<WgsPoint> waypoints)
    {
        var (b, e) = await RoutingEngineFetcher.GetBody(ValhallaQueryConstructor.Table(addr, waypoints));

        if (e is not null) { return (null, new() { message = e }); }

        if (b is null) { return (null, null); }

        try {
            var ans = JsonSerializer.Deserialize<Answer>(b);

            return (new() {
                distances = ans.sources_to_targets.Select(row => row.Select(col => col.distance * 1000.0).ToList()).ToList()
            }, null);
        }
        catch (Exception ex) { return (null, new() { message = ex.Message }); }
    }
}
