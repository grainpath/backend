using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Entity;

namespace backend.RoutingEngine;

sealed class OsrmRoutingEngine : IRoutingEngine
{
    private static readonly string _addr = @"https://routing.openstreetmap.de/routed-foot/";

    public async Task<ShortHandle> GetShortHandle(List<WebPoint> sequence)
        => await OsrmShortestPathFinder.Find(_addr, sequence);
}
