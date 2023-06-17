using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Handlers;

/// <summary>
/// <c>/direcs</c> request handler.
/// </summary>
public static class DirecsHandler
{
    /// <summary>
    /// Get a list of possible directions.
    /// </summary>
    public static Task<(List<ShortestPathObject>, ErrorObject)> Handle(IRoutingEngine engine, List<WebPoint> waypoints)
    {
        var points = waypoints.Select(w => w.AsWgs()).ToList();
        return engine.GetShortestPath(points);
    }
}
