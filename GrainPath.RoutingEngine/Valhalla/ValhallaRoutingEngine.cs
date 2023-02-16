using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.RoutingEngine.Valhalla;

public sealed class ValhallaRoutingEngine : IRoutingEngine
{
    private static readonly string _addr;

    static ValhallaRoutingEngine() { _addr = System.Environment.GetEnvironmentVariable("GRAINPATH_REV_ADDR"); }

    public async Task<ShortObject> HandleShort(List<WebPoint> sequence)
        => await ValhallaShortestPathFinder.Find(_addr, sequence);
}
