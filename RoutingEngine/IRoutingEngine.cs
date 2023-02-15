using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Entity;

namespace backend.RoutingEngine;

internal interface IRoutingEngine
{
    Task<ShortestPath> GetShortestPath(List<WebPoint> sequence);
}
