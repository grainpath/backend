using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Entity;

namespace backend.RoutingEngine;

sealed class ValhallaRoutingEngine : IRoutingEngine
{
    private static readonly string _addr;

    static ValhallaRoutingEngine() { _addr = System.Environment.GetEnvironmentVariable("GRAINPATH_RE_ADDR"); }

    public async Task<ShortObject> HandleShort(List<WebPoint> sequence)
        => await ValhallaShortestPathFinder.Find(_addr, sequence);
}
