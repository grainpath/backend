using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Entity;

namespace backend.RoutingEngine;

sealed class OsrmRoutingEngine : IRoutingEngine
{
    private static readonly string _addr;

    static OsrmRoutingEngine() { _addr = System.Environment.GetEnvironmentVariable("GRAINPATH_RE_ADDR"); }

    public async Task<ShortHandle> GetShortHandle(List<WebPoint> sequence)
        => await OsrmShortestPathFinder.Find(_addr, sequence);
}
