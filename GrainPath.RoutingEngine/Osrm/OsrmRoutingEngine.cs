using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;
using GrainPath.RoutingEngine.Osrm.Actions;

namespace GrainPath.RoutingEngine.Osrm;

internal sealed class OsrmRoutingEngine : IRoutingEngine
{
    private readonly string _addr;

    internal OsrmRoutingEngine(string addr) { _addr = addr; }

    public async Task<(ShortestPathObject, ErrorObject)> GetShortestPath(List<WgsPoint> waypoints)
        => await OsrmShortestPath.Get(_addr, waypoints);

    public async Task<(DistanceMatrixObject, ErrorObject)> GetDistanceMatrix(List<WgsPoint> waypoints)
        => await OsrmDistanceMatrix.Get(_addr, waypoints);
}
