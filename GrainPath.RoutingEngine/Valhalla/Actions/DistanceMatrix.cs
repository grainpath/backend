using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.RoutingEngine.Valhalla.Actions;

internal static class DistanceMatrix
{
    private sealed class Query
    {
        internal sealed class PedestrianOptions
        {
            public bool shortest { get; set; } = true;
        }

        internal sealed class CostingOptions
        {
            public PedestrianOptions pedestrian { get; set; } = new();
        }

        public string units => "kilometers";

        public string costing => "pedestrian";

        public CostingOptions costing_options { get; set; } = new();

        public List<WebPoint> sources { get; set; }

        public List<WebPoint> targets { get; set; }
    }

    private sealed class Answer
    {
        internal sealed class Item
        {
            public double distance { get; set; }
        }

        public List<List<Item>> sources_to_targets { get; set; }
    }

    private static string _prefix = "/sources_to_targets?json=";

    public static async Task<(DistanceMatrixObject, ErrorObject)> Act(string addr, List<WebPoint> points)
    {
        var suffix = new Query() { sources = points, targets = points };
        var query = addr + _prefix + JsonSerializer.Serialize(suffix);

        var (b, e) = await RoutingEngineFetcher.GetBody(query);

        if (e is not null) { return (null, new() { message = e }); }

        if (b is null) { return (null, null); }

        try {
            var ans = JsonSerializer.Deserialize<Answer>(b);

            return (new() {
                distances = ans.sources_to_targets.Select(row => row.Select(col => col.distance).ToList()).ToList()
            }, null);
        }
        catch (Exception ex) { return (null, new() { message = ex.Message }); }
    }
}
