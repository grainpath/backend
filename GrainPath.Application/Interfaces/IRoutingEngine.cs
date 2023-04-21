using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Interfaces;

public interface IRoutingEngine
{
    /// <summary>
    /// Calculate polyline, distance, and duration of the fastest route
    /// visiting waypoints in a given order.
    /// </summary>
    public Task<(ShortestPathObject, ErrorObject)> GetShortestPath(List<WgsPoint> waypoints);

    /// <summary>
    /// Calculate distance matrix, distance in meters between all pairs of points.
    /// </summary>
    public Task<(DistanceMatrixObject, ErrorObject)> GetDistanceMatrix(List<WgsPoint> waypoints);
}
