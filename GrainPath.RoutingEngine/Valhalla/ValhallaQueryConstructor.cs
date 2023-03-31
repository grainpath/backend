using System.Collections.Generic;
using System.Text.Json;
using GrainPath.Application.Entities;

namespace GrainPath.RoutingEngine.Valhalla;

internal static class ValhallaQueryConstructor
{
    private sealed class PedestrianOptions
    {
        public bool shortest { get; set; } = true;

        public bool disable_hierarchy_pruning { get; set; } = true;
    }

    private sealed class CostingOptions
    {
        public PedestrianOptions pedestrian { get; set; } = new();
    }

    private class Query
    {
        public string units => "kilometers";

        public string costing => "pedestrian";

        public CostingOptions costing_options { get; set; } = new();
    }

    private sealed class RouteQuery
    {
        public List<WgsPoint> locations { get; set; }
    }

    private sealed class TableQuery
    {
        public List<WgsPoint> sources { get; set; }

        public List<WgsPoint> targets { get; set; }
    }

    public static string Route(string addr, List<WgsPoint> points)
        => addr + "/route?json=" + JsonSerializer.Serialize(new RouteQuery() { locations = points });

    public static string Table(string addr, List<WgsPoint> points)
        => addr + "/sources_to_targets?json=" + JsonSerializer.Serialize(new TableQuery() { sources = points, targets = points });
}
