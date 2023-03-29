using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;
using Microsoft.Extensions.Logging;

namespace GrainPath.Application.Interfaces;

public interface IRoutingEngine
{
    public Task<(ShortestPathObject, ErrorObject)> GetShortestPath(List<WebPoint> sequence);

    public Task<(DistanceMatrixObject, ErrorObject)> GetDistanceMatrix();
}
