using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Entity;
using backend.RoutingEngine;

namespace backend.RequestHandler;

internal static class ShortRequestHandler
{
    public static async Task<ShortestPath> Handle(ShortRequest request)
    {
        var ps = new List<WebPoint>();

        ps.Add(request.source);
        ps.AddRange(request.sequence);
        ps.Add(request.target);

        return await RoutingEngineFactory.GetInstance().GetShortestPath(ps);
    }
}
