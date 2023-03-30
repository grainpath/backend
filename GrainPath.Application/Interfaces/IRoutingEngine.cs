using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Interfaces;

public interface IRoutingEngine
{
    public Task<(ShortestPathObject, ErrorObject)> GetShortestPath(List<WgsPoint> waypoints);

    public Task<(DistanceMatrixObject, ErrorObject)> GetDistanceMatrix(List<WgsPoint> waypoints);
}
