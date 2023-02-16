using GrainPath.Application.Interfaces;

namespace GrainPath.Application.Entities;

public class ShortObject
{
    public ShortResponse response { get; set; }

    public RoutingEngineStatus status { get; set; }

    public string message { get; set; }
}
