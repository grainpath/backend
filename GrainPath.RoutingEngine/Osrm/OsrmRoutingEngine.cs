using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;
using GrainPath.RoutingEngine.Osrm.Fetchers;

namespace GrainPath.RoutingEngine.Osrm;

internal sealed class OsrmRoutingEngine : IRoutingEngine
{
    private readonly string _addr;

    internal OsrmRoutingEngine(string addr) { _addr = addr; }

    public async Task<(List<ShortestPathObject>, ErrorObject)> GetShortestPath(List<WgsPoint> waypoints)
        => await ShortestPathFetcher.Fetch(_addr, waypoints);

    public async Task<(IDistanceMatrix, ErrorObject)> GetDistanceMatrix(List<WgsPoint> waypoints)
        => await DistanceMatrixFetcher.Fetch(_addr, waypoints);
}
