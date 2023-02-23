using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Handlers;

public static class ShortHandler
{
    public static async Task<ShortObject> Handle(IRoutingEngine engine, ShortRequest request)
    {
        var ps = new List<WebPoint>();

        ps.Add(request.source);
        ps.AddRange(request.sequence);
        ps.Add(request.target);

        return await engine.GetShortestPath(ps);
    }
}
