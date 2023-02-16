using System.Collections.Generic;
using backend.RoutingEngine;

namespace backend.Entity;

class ShortPayload
{
    /// <summary>
    /// Distance of the shortest path in <b>meters</b>.
    /// </summary>
    internal double distance { get; set; }

    internal List<WebPoint> shape { get; set; }
}

class ShortObject
{
    public ShortPayload payload { get; set; }

    public string message { get; set; }

    public RoutingEngineStatus status { get; set; }
}
