using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;
using GrainPath.RoutingEngine.Osrm.Actions;

namespace GrainPath.RoutingEngine.Osrm;

public sealed class OsrmRoutingEngine : IRoutingEngine
{
    private readonly string _addr;

    public OsrmRoutingEngine(string addr) { _addr = addr; }

    public async Task<ShortObject> GetShortestPath(List<WebPoint> sequence)
        => await ShortestPath.Act(_addr, sequence);
}
