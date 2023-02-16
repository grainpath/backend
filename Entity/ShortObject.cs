using System.Collections.Generic;
using backend.RoutingEngine;

namespace backend.Entity;

class ShortestPath
{
    internal double distance { get; set; }

    internal List<WebPoint> route { get; set; }
}

class ShortObject
{
    public string message { get; set; }

    public RoutingEngineStatus status { get; set; }

    public ShortestPath path { get; set; }
}
