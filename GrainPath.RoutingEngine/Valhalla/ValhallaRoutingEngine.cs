using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;
using GrainPath.RoutingEngine.Valhalla.Actions;

namespace GrainPath.RoutingEngine.Valhalla;

public sealed class ValhallaRoutingEngine : IRoutingEngine
{
    private readonly string _addr;

    public ValhallaRoutingEngine(string addr) { _addr = addr; }

    public async Task<(ShortestPathObject, ErrorObject)> GetShortestPath(List<WebPoint> waypoints)
        => await ShortestPath.Act(_addr, waypoints);

    public async Task<(DistanceMatrixObject, ErrorObject)> GetDistanceMatrix(List<WebPoint> points)
        => await DistanceMatrix.Act(_addr, points);
}
