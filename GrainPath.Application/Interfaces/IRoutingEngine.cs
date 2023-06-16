using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Interfaces;

public interface IRoutingEngine
{
    /// <summary>
    /// Calculate distance matrix, distance in meters between all pairs of points.
    /// </summary>
    public Task<(IDistanceMatrix, ErrorObject)> GetDistanceMatrix(List<WgsPoint> waypoints);

    /// <summary>
    /// Calculate polyline, distance, and duration of the fastest routes
    /// visiting waypoints in a given order.
    /// </summary>
    public Task<(List<ShortestPathObject>, ErrorObject)> GetShortestPath(List<WgsPoint> waypoints);
}
