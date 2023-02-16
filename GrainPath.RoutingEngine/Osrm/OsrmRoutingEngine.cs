using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.RoutingEngine.Osrm;

public sealed class OsrmRoutingEngine : IRoutingEngine
{
    private static readonly string _addr;

    static OsrmRoutingEngine() { _addr = System.Environment.GetEnvironmentVariable("GRAINPATH_REO_ADDR"); }

    public async Task<ShortObject> HandleShort(List<WebPoint> sequence)
        => await OsrmShortestPathFinder.Find(_addr, sequence);
}
