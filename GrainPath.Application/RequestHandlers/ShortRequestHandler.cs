using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.RequestHandlers;

public static class ShortRequestHandler
{
    public static async Task<ShortObject> Handle(IRoutingEngine engine, ShortRequest request)
    {
        var ps = new List<WebPoint>();

        ps.Add(request.source);
        ps.AddRange(request.sequence);
        ps.Add(request.target);

        return await engine.HandleShort(ps);
    }
}
