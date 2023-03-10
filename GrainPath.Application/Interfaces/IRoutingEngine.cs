using System.Collections.Generic;
using System.Threading.Tasks;
using GrainPath.Application.Entities;

namespace GrainPath.Application.Interfaces;

public enum RoutingEngineStatus
{
    OK = 200,

    BR = 400,   // bad response

    UN = 500,   // unavailable
}

public interface IRoutingEngine
{
    public Task<ShortObject> GetShortestPath(List<WebPoint> sequence);
}
