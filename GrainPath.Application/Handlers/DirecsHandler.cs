using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

public static class DirecsHandler
{
    public static Task<(List<ShortestPathObject>, ErrorObject)> GetDirecs(IRoutingEngine engine, List<WebPoint> waypoints)
    {
        var points = waypoints.Select(w => w.AsWgs()).ToList();
        return engine.GetShortestPath(points);
    }
}
